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

            //  Generar un número de transacción único
            int nuevoNumeroTransaccion;
            string sqlObtenerUltimo = "SELECT ISNULL(MAX(NumeroTransaccion), 0) + 1 FROM Transacciones";

            using (var cmd = new SqlCommand(sqlObtenerUltimo, conexion))
            {
                nuevoNumeroTransaccion = (int)cmd.ExecuteScalar();
            }

            transaccion.NumeroTransaccion = nuevoNumeroTransaccion; // Asignar el número generado

            //  Validaciones: verificar si las cuentas existen y saldo suficiente
            string sqlVerificar = "SELECT Saldo FROM Cuentas WHERE CuentaID = @CuentaID";
            decimal saldoCuentaOrigen;

            using (var comandoVerificar = new SqlCommand(sqlVerificar, conexion))
            {
                comandoVerificar.Parameters.AddWithValue("@CuentaID", transaccion.CuentaOrigenID);
                object resultado = comandoVerificar.ExecuteScalar();
                if (resultado == null)
                {
                    throw new Exception("La cuenta de origen no existe.");
                }
                saldoCuentaOrigen = Convert.ToDecimal(resultado);
            }

            //  Verificar saldo suficiente
            if (saldoCuentaOrigen < transaccion.Monto)
            {
                throw new Exception("Saldo insuficiente para realizar la transacción.");
            }

            //  Insertar la transacción
            string sqlInsert = @"INSERT INTO Transacciones (NumeroTransaccion, Fecha, CuentaOrigenID, CuentaDestinoID, Monto, Tipo) 
                             VALUES (@NumeroTransaccion, @Fecha, @CuentaOrigenID, @CuentaDestinoID, @Monto, @Tipo)";

            using (var comandoInsert = new SqlCommand(sqlInsert, conexion))
            {
                comandoInsert.Parameters.AddWithValue("@NumeroTransaccion", transaccion.NumeroTransaccion);
                comandoInsert.Parameters.AddWithValue("@Fecha", DateTime.Now);
                comandoInsert.Parameters.AddWithValue("@CuentaOrigenID", transaccion.CuentaOrigenID);
                comandoInsert.Parameters.AddWithValue("@CuentaDestinoID", transaccion.CuentaDestinoID);
                comandoInsert.Parameters.AddWithValue("@Monto", transaccion.Monto);
                comandoInsert.Parameters.AddWithValue("@Tipo", transaccion.Tipo);

                comandoInsert.ExecuteNonQuery();
            }

            return true;
        }
    }


    public List<Transacciones> ConsultarTransacciones(int cuentaID, DateTime? desde, DateTime? hasta)
    {
        var transacciones = new List<Transacciones>();
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            string sql = "SELECT * FROM Transacciones WHERE CuentaOrigenID = @CuentaID OR CuentaDestinoID = @CuentaID";

            if (desde.HasValue && hasta.HasValue)
            {
                sql += " AND Fecha BETWEEN @Desde AND @Hasta";
            }

            using (var cmd = new SqlCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@CuentaID", cuentaID);
                if (desde.HasValue && hasta.HasValue)
                {
                    cmd.Parameters.AddWithValue("@Desde", desde.Value);
                    cmd.Parameters.AddWithValue("@Hasta", hasta.Value);
                }

                conexion.Open();
                using (var lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        transacciones.Add(new Transacciones
                        {
                            NumeroTransaccion = Convert.ToInt32(lector["NumeroTransaccion"]),
                            Fecha = Convert.ToDateTime(lector["Fecha"]),
                            CuentaOrigenID = Convert.ToInt32(lector["CuentaOrigenID"]),
                            CuentaDestinoID = Convert.ToInt32(lector["CuentaDestinoID"]),
                            Monto = Convert.ToDecimal(lector["Monto"]),
                            Tipo = lector["Tipo"].ToString()!
                        });
                    }
                }
            }
        }
        return transacciones;
    }


    public List<Transacciones> ListarTodasTransacciones()
    {
        var transacciones = new List<Transacciones>();

        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            string sql = "SELECT * FROM Transacciones";

            using (var comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                var lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    var transaccion = new Transacciones
                    {
                        NumeroTransaccion = lector["NumeroTransaccion"] != DBNull.Value ? Convert.ToInt32(lector["NumeroTransaccion"]) : 0,
                        Fecha = lector["Fecha"] != DBNull.Value ? Convert.ToDateTime(lector["Fecha"]) : (DateTime?)null,
                        CuentaOrigenID = lector["CuentaOrigenID"] != DBNull.Value ? Convert.ToInt32(lector["CuentaOrigenID"]) : 0,
                        CuentaDestinoID = lector["CuentaDestinoID"] != DBNull.Value ? Convert.ToInt32(lector["CuentaDestinoID"]) : 0,
                        Monto = lector["Monto"] != DBNull.Value ? Convert.ToDecimal(lector["Monto"]) : 0,
                        Tipo = lector["Tipo"] != DBNull.Value ? lector["Tipo"].ToString()! : string.Empty
                    };

                    transacciones.Add(transaccion); 
                }
            }
        }

        return transacciones; 
    }
}
