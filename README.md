# Sistema de Gestión de Solicitudes Académicas

Proyecto en C#/.NET 10 usado para el Trabajo Práctico Integrador de Metodología de Sistemas II.

## Estructura

- `src/Solicitudes.Core`: dominio, casos de uso, puertos y adaptador en memoria.
- `tests/Solicitudes.Core.Tests`: pruebas unitarias xUnit.
- `codigo_antes/CodigoAntes.cs`: línea base deliberadamente acoplada que se analiza en el informe.
- `docs`: diagnóstico, checklist de revisión técnica y evidencias reproducibles.

## Documentación y evidencias

- [Checklist de revisión técnica](docs/checklist-revision-tecnica.md)
- [Diagnóstico de deuda técnica y code smells](docs/diagnostico.md)
- [Índice de evidencias](docs/evidencias/README.md)
- [Resultado de pruebas unitarias](docs/evidencias/pruebas-unitarias.txt)

## Ejecutar las pruebas

Desde esta carpeta:

```powershell
dotnet test .\tests\Solicitudes.Core.Tests\Solicitudes.Core.Tests.csproj
```

La persistencia del prototipo es en memoria y se reinicia al iniciar la aplicación. No se incluyen interfaz gráfica, API ni base de datos.
