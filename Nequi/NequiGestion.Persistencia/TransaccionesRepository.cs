using Microsoft.Data.SqlClient;
using NequiGestion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Persistencia;

public class TransaccionesRepository
{
    private readonly string _cadena_conexion = "Server=DESKTOP-5NB7DNF\\SQLEXPRESS;Database=Necli;Trusted_Connection=True; TrustServerCertificate=True;";
    private string sql;

    public bool RegistrarTransaccion(Transacciones transaccion)
    {
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            conexion.Open();

            // Verificar si las cuentas existen antes de insertar la transacción
            string sqlVerificar = "SELECT COUNT(*) FROM Cuentas WHERE CuentaID = @CuentaID";

            using (var comandoVerificar = new SqlCommand(sqlVerificar, conexion))
            {
                // Verificar CuentaOrigenID
                comandoVerificar.Parameters.AddWithValue("@CuentaID", transaccion.CuentaOrigenID);
                int cuentaOrigenExiste = (int)comandoVerificar.ExecuteScalar();

                // Verificar CuentaDestinoID
                comandoVerificar.Parameters["@CuentaID"].Value = transaccion.CuentaDestinoID;
                int cuentaDestinoExiste = (int)comandoVerificar.ExecuteScalar();

                if (cuentaOrigenExiste == 0 || cuentaDestinoExiste == 0)
                {
                    throw new Exception("Una o ambas cuentas no existen en la base de datos.");
                }
            }

            // Si ambas cuentas existen, proceder con la inserción de la transacción
            string sqlInsert = @"INSERT INTO Transacciones (NumeroTransaccion, Fecha, CuentaOrigenID, CuentaDestinoID, Monto, Tipo) 
                             VALUES (@NumeroTransaccion, @Fecha, @CuentaOrigenID, @CuentaDestinoID, @Monto, @Tipo)";

            using (var comandoInsert = new SqlCommand(sqlInsert, conexion))
            {
                comandoInsert.Parameters.AddWithValue("@NumeroTransaccion", transaccion.NumeroTransaccion);
                comandoInsert.Parameters.AddWithValue("@Fecha", transaccion.Fecha ?? DateTime.Now);
                comandoInsert.Parameters.AddWithValue("@CuentaOrigenID", transaccion.CuentaOrigenID);
                comandoInsert.Parameters.AddWithValue("@CuentaDestinoID", transaccion.CuentaDestinoID);
                comandoInsert.Parameters.AddWithValue("@Monto", transaccion.Monto);
                comandoInsert.Parameters.AddWithValue("@Tipo", transaccion.Tipo);

                comandoInsert.ExecuteNonQuery();   
            }
        }

        return true;
    }

    public Transacciones ConsultarTransaccion(int NumeroTransaccion)
    {
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            sql = "SELECT * FROM Transacciones WHERE NumeroTransaccion = @NumeroTransaccion;";

            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@NumeroTransaccion", NumeroTransaccion);
                conexion.Open();
                var lector = comando.ExecuteReader();

                if (lector.Read())
                {
                    return new Transacciones
                    {
                        NumeroTransaccion = Convert.ToInt32(lector["NumeroTransaccion"]),
                        Fecha = lector["Fecha"] as DateTime?,
                        CuentaOrigenID = Convert.ToInt32(lector["CuentaOrigenID"]),
                        CuentaDestinoID = Convert.ToInt32(lector["CuentaDestinoID"]),
                        Monto = Convert.ToDecimal(lector["Monto"]),
                        Tipo = lector["Tipo"].ToString()!
                    };
                }
            }
        }
        return null;
    }

    public List<Transacciones> ListarTransacciones()
    {
        var transacciones = new List<Transacciones>();

        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            string sql = "SELECT * FROM Transacciones";
            using (var comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        transacciones.Add(new Transacciones
                        {
                            NumeroTransaccion = Convert.IsDBNull(lector["NumeroTransaccion"]) ? 0 : Convert.ToInt32(lector["NumeroTransaccion"]),
                            Fecha = Convert.IsDBNull(lector["Fecha"]) ? DateTime.MinValue : Convert.ToDateTime(lector["Fecha"]),
                            CuentaOrigenID = Convert.IsDBNull(lector["CuentaOrigenID"]) ? 0 : Convert.ToInt32(lector["CuentaOrigenID"]),
                            CuentaDestinoID = Convert.IsDBNull(lector["CuentaDestinoID"]) ? 0 : Convert.ToInt32(lector["CuentaDestinoID"]),
                            Monto = Convert.IsDBNull(lector["Monto"]) ? 0.0m : Convert.ToDecimal(lector["Monto"]),
                            Tipo = Convert.IsDBNull(lector["Tipo"]) ? string.Empty : lector["Tipo"].ToString()
                        });
                    }
                }
            }
        }

        return transacciones;
    }

    public List<Transacciones> ListarTransaccionesPorNumero(int numeroTransaccion)
    {
        var transacciones = new List<Transacciones>();
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            sql = "SELECT * FROM Transacciones WHERE NumeroTransaccion = @NumeroTransaccion";
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@NumeroTransaccion", numeroTransaccion);
                conexion.Open();
                var lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    transacciones.Add(new Transacciones
                    {
                        NumeroTransaccion = Convert.ToInt32(lector["NumeroTransaccion"]),
                        Fecha = lector["Fecha"] as DateTime?,
                        CuentaOrigenID = Convert.ToInt32(lector["CuentaOrigenID"]),
                        CuentaDestinoID = Convert.ToInt32(lector["CuentaDestinoID"]),
                        Monto = Convert.ToDecimal(lector["Monto"]),
                        Tipo = lector["Tipo"].ToString()!
                    });
                }
            }
        }
        return transacciones;
    }
}
