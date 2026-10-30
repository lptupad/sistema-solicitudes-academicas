# Checklist de revisión técnica

Proyecto: Sistema de Gestión de Solicitudes Académicas  
Alcance revisado: prototipo C#/.NET 10, núcleo de dominio/aplicación y pruebas unitarias.  
Tipo de revisión: auto-revisión técnica del trabajo práctico.

## Checklist aplicado

| # | Criterio | Resultado | Evidencia / observación |
|---|---|---|---|
| 1 | Las clases tienen responsabilidades identificables y acotadas. | Cumple | Dominio en `src/Solicitudes.Core/Domain`; casos de uso en `Application`; puertos en `Ports`; adaptador en `Infrastructure`. |
| 2 | Los nombres describen el dominio y las operaciones. | Cumple | `Solicitud`, `RegistrarSolicitudCommand`, `CambiarEstado`, `GenerarReporte`. |
| 3 | La lógica de negocio evita depender directamente de infraestructura concreta. | Cumple | `SolicitudService` recibe `ISolicitudRepository` e `IClock`; `SolicitudQueryService` recibe `ISolicitudReadRepository`. |
| 4 | Las interfaces son pequeñas y orientadas a los consumidores. | Cumple | Separación de lectura y escritura entre `ISolicitudReadRepository` e `ISolicitudRepository`. |
| 5 | Los errores y entradas inválidas se informan explícitamente. | Cumple | Validaciones con `ArgumentException`; solicitud inexistente con `KeyNotFoundException`; transición no permitida con `InvalidOperationException`. |
| 6 | Las reglas de estado quedan protegidas por el dominio. | Cumple | `Solicitud.CambiarEstado` valida la transición antes de modificar el estado. |
| 7 | Las pruebas cubren operaciones y escenarios de error importantes. | Cumple parcialmente | 9 casos ejecutados; consultar el detalle en `evidencias/pruebas-unitarias.txt`. No hay pruebas de integración, concurrencia ni persistencia durable. |
| 8 | El reloj puede controlarse en las pruebas. | Cumple | `IClock` y `FixedClock` permiten fijar la fecha de creación. |
| 9 | La ejecución de pruebas es repetible desde una instrucción documentada. | Cumple | `dotnet test .\tests\Solicitudes.Core.Tests\Solicitudes.Core.Tests.csproj`. |
| 10 | La persistencia satisface el alcance declarado y sus límites están documentados. | Cumple con limitación conocida | Adaptador en memoria; los datos no sobreviven al proceso y no se garantiza acceso concurrente. |
| 11 | La estructura contiene instrucciones de instalación/uso y documentación de revisión. | Cumple | README del proyecto y este checklist. |
| 12 | El repositorio remoto, los tags de entrega y las evidencias visuales están publicados. | Cumple | Repositorio público verificado: https://github.com/lptupad/sistema-solicitudes-academicas. Rama `main` y seis tags publicados. Capturas reales en `docs/evidencias`: estructura, historial de commits y tags. |

## Observaciones y acciones

1. **Persistencia:** el adaptador en memoria es suficiente para simular almacenamiento en este prototipo, pero no se debe presentar como persistencia durable ni como listo para producción.
2. **Pruebas:** la suite valida los casos principales del alcance. Al extender el sistema, agregar pruebas para filtros individuales/combinados, orden por fecha, reglas adicionales de transición y el adaptador persistente que se incorpore.
3. **Errores en una interfaz futura:** las excepciones de dominio/aplicación deberán traducirse en el límite de una API o interfaz, conservando el detalle técnico en logs y evitando mensajes genéricos silenciosos.

## Criterio de aceptación de esta revisión

- El proyecto compila al ejecutar las pruebas.
- Todos los tests de la suite actual pasan.
- Las limitaciones del prototipo y los elementos pendientes de publicación están declarados, sin presentar evidencia no generada como si existiera.
