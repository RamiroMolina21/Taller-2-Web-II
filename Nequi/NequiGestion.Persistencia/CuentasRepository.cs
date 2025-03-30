using NequiGestion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace NequiGestion.Persistencia;

public class CuentasRepository {
    private readonly string _cadena_conexion = "Server=DESKTOP-5NB7DNF\\SQLEXPRESS;Database=Necli;Trusted_Connection=True; TrustServerCertificate=True;";
    private string sql;

    public bool RegistrarCuenta(Cuentas cuenta) {

        using(var conexion = new SqlConnection(_cadena_conexion)) {

            sql = @"INSERT INTO Cuentas(CuentaID, Nombre, Apellido, Email, Telefono, Saldo, FechaCreacion, ContrasenaHash)
                    VALUES(@CuentaID, @Nombre, @Apellido, @Email, @Telefono, @Saldo, @FechaCreacion, @ContrasenaHash)";

            using (var comando = new SqlCommand(sql, conexion)) {

                comando.Parameters.AddWithValue("@CuentaID", cuenta.CuentaID);
                comando.Parameters.AddWithValue("@Nombre", cuenta.Nombre);
                comando.Parameters.AddWithValue("@Apellido", cuenta.Apellido);
                comando.Parameters.AddWithValue("@Email", cuenta.Email);
                comando.Parameters.AddWithValue("@Telefono", cuenta.Telefono);
                comando.Parameters.AddWithValue("@Saldo", cuenta.Saldo);
                comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                comando.Parameters.AddWithValue("@ContrasenaHash", cuenta.ContrasenaHash);
                conexion.Open();
                comando.ExecuteNonQuery();

            }

        }

        return true;

    }

    public Cuentas ConsultarCuenta(int CuentaID) {

        using (var conexion = new SqlConnection(_cadena_conexion)){

            sql = "SELECT CuentaID, Nombre, Apellido, Email, Telefono, Saldo, FechaCreacion FROM Cuentas WHERE CuentaID = @CuentaID;";

            using (var comando = new SqlCommand(sql, conexion)) {

                comando.Parameters.AddWithValue("@CuentaID", CuentaID);
                conexion.Open();
                var lector = comando.ExecuteReader();

                while (lector.Read()) {

                    var cuenta = new Cuentas
                    {
                        CuentaID = Convert.ToInt32(lector["CuentaID"].ToString()),
                        Nombre = lector["Nombre"].ToString(),
                        Apellido = lector["Apellido"].ToString(),
                        Email = lector["Email"].ToString(),
                        Telefono = lector["Telefono"].ToString(),
                        Saldo = Convert.ToDecimal(lector["Saldo"].ToString()),
                        FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString()),
                    };
                    return cuenta; 

                }

            }
        }

        return null;

    }


    public Cuentas ConsultarCuentaTelefono(string Telefono)
    {

        using (var conexion = new SqlConnection(_cadena_conexion))
        {

            sql = "SELECT CuentaID, Nombre, Apellido, Email, Telefono, Saldo, FechaCreacion FROM Cuentas WHERE Telefono = @Telefono;";

            using (var comando = new SqlCommand(sql, conexion))
            {

                comando.Parameters.AddWithValue("@Telefono", Telefono);
                conexion.Open();
                var lector = comando.ExecuteReader();

                while (lector.Read())
                {

                    var cuenta = new Cuentas
                    {
                        CuentaID = Convert.ToInt32(lector["CuentaID"].ToString()),
                        Nombre = lector["Nombre"].ToString(),
                        Apellido = lector["Apellido"].ToString(),
                        Email = lector["Email"].ToString(),
                        Telefono = lector["Telefono"].ToString(),
                        Saldo = Convert.ToDecimal(lector["Saldo"].ToString()),
                        FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString()),
                    };
                    return cuenta;

                }

            }
        }

        return null;

    }



    public List<Cuentas> ListarTodasCuentas()
    {

        var cuentas = new List<Cuentas>();

        using (var conexion = new SqlConnection(_cadena_conexion))
        {

            sql = "SELECT * FROM Cuentas";

            using (var comando = new SqlCommand(sql, conexion))
            {

               
                conexion.Open();
                var lector = comando.ExecuteReader();


                while (lector.Read())
                {

                    var cuenta = new Cuentas
                    {
                        CuentaID = Convert.ToInt32(lector["CuentaID"].ToString()),
                        Nombre = lector["Nombre"].ToString(),
                        Apellido = lector["Apellido"].ToString(),
                        Email = lector["Email"].ToString(),
                        Telefono = lector["Telefono"].ToString(),
                        Saldo = Convert.ToDecimal(lector["Saldo"].ToString()),
                        FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString()),
                        ContrasenaHash = lector["ContrasenaHash"].ToString()
                    };
                    cuentas.Add(cuenta);

                } 

            }
        }



        return cuentas;
    }

    public bool EliminarCuenta(string Telefono)
    {
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            conexion.Open();

            // Primero, eliminar las transacciones asociadas
            using (var comandoTransacciones = new SqlCommand("DELETE FROM Transacciones WHERE CuentaOrigenID = @CuentaID OR CuentaDestinoID = @CuentaID", conexion))
            {
                comandoTransacciones.Parameters.AddWithValue("@CuentaID", Telefono);
                comandoTransacciones.ExecuteNonQuery();
            }

            // Ahora sí, eliminar la cuenta
            using (var comandoCuenta = new SqlCommand("DELETE FROM Cuentas WHERE Telefono = @Telefono", conexion))
            {
                comandoCuenta.Parameters.AddWithValue("@Telefono", Telefono);
                return comandoCuenta.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool ActualizarCuenta(Cuentas cuenta)
    {
        using (var conexion = new SqlConnection(_cadena_conexion))
        {
            sql = @"UPDATE Cuentas 
                SET Nombre = @Nombre, 
                    Apellido = @Apellido, 
                    Email = @Email, 
                    Telefono = @Telefono, 
                    Saldo = @Saldo, 
                    FechaCreacion = @FechaCreacion, 
                    ContrasenaHash = @ContrasenaHash 
                WHERE CuentaID = @CuentaID";

            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@CuentaID", cuenta.CuentaID);
                comando.Parameters.AddWithValue("@Nombre", cuenta.Nombre);
                comando.Parameters.AddWithValue("@Apellido", cuenta.Apellido);
                comando.Parameters.AddWithValue("@Email", cuenta.Email);
                comando.Parameters.AddWithValue("@Telefono", cuenta.Telefono);
                comando.Parameters.AddWithValue("@Saldo", cuenta.Saldo);
                comando.Parameters.AddWithValue("@FechaCreacion", cuenta.FechaCreacion);
                comando.Parameters.AddWithValue("@ContrasenaHash", cuenta.ContrasenaHash);

                conexion.Open();
                int filasAfectadas = comando.ExecuteNonQuery();

                // Si se actualizó al menos una fila, retorna true
                return filasAfectadas > 0;
            }
        }
    }

    
}
