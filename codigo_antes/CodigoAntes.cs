using System;
using System.Collections.Generic;

namespace Solicitudes.Antes;

// Línea base deliberadamente problemática para el análisis del TP.
public sealed class GestorSolicitudes
{
    private static readonly List<string[]> Datos = new();

    public string Procesar(
        string accion, string id, string asunto, string descripcion, string tipo,
        string prioridad, string idEstudiante, string nombreEstudiante,
        string email, string idResponsable, string nombreResponsable, string area)
    {
        try
        {
            if (accion == "REG")
            {
                if (asunto == "" || descripcion == "") return "Error";
                Datos.Add(new[]
                {
                    Guid.NewGuid().ToString(), 
                    asunto, 
                    descripcion, 
                    tipo, 
                    prioridad,
                    "Pendiente", 
                    idEstudiante + "|" + nombreEstudiante + "|" + email,
                    "", DateTime.Now.ToString()
                });
                return "OK";
            }

            if (accion == "ESTADO" || accion == "ASIG")
            {
                foreach (var s in Datos)
                {
                    if (s[0] == id)
                    {
                        if (accion == "ESTADO") s[5] = prioridad;
                        if (accion == "ASIG")
                            s[7] = idResponsable + "|" + nombreResponsable + "|" + area;
                        return "OK";
                    }
                }
                return "Error";
            }

            if (accion == "BUSCAR")
            {
                var resultado = "";
                foreach (var s in Datos)
                    if (s[1].ToLower().Contains(asunto.ToLower()) || s[5] == prioridad)
                        resultado += s[0] + ";" + s[1] + ";" + s[5] + "\n";
                return resultado;
            }

            if (accion == "REPORTE")
            {
                var pendientes = 0;
                var proceso = 0;
                var resueltas = 0;
                foreach (var s in Datos)
                {
                    if (s[5] == "Pendiente") pendientes++;
                    if (s[5] == "EnProceso") proceso++;
                    if (s[5] == "Resuelta") resueltas++;
                }
                return "Pendientes=" + pendientes + ", En proceso=" + proceso
                    + ", Resueltas=" + resueltas;
            }

            return "Error";
        }
        catch
        {
            return "Error";
        }
    }
}
