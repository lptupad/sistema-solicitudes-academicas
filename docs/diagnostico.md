# Diagnóstico de deuda técnica y code smells

## Alcance

Se revisó la línea base de `codigo_antes/CodigoAntes.cs`, en particular `GestorSolicitudes.Procesar`. El análisis se limita al prototipo de gestión de solicitudes y a los riesgos observables en ese archivo; no implica una auditoría de seguridad ni de rendimiento.

## Hallazgos

| # | Code smell / evidencia | Impacto y riesgo | Prioridad |
|---|---|---|---|
| 1 | **God Object / Large Class:** `GestorSolicitudes.Procesar` registra, cambia estados, asigna responsables, busca y genera reportes. | Varias razones de cambio, efectos colaterales y dificultad para aislar pruebas. | Alta |
| 2 | **Long Parameter List:** `Procesar` recibe 12 argumentos, aunque cada acción usa solo algunos. | Las llamadas pueden invertir valores y quedan acopladas a detalles de operaciones distintas. | Alta |
| 3 | **Primitive Obsession / datos posicionales:** cada solicitud se representa con `string[]`; posiciones como `s[5]` y `s[7]` codifican estado y responsable. | Un índice incorrecto o un cambio en el orden de los datos puede corromper su interpretación sin error de compilación. | Alta |
| 4 | **Magic Strings:** acciones, estados y prioridades se representan mediante literales de texto. | Errores tipográficos y cambios de nomenclatura requieren localizar usos manualmente. | Media |
| 5 | **Condicionales por tipo de acción:** una cadena de condiciones sobre `accion` controla todo el flujo. | Incorporar operaciones aumenta las ramas y obliga a modificar un método de alto riesgo. | Alta |
| 6 | **Manejo de errores opaco:** el `catch` general devuelve `"Error"` para cualquier excepción. | Se pierden causa y contexto, y el llamador no puede distinguir errores de validación de otros fallos. | Alta |
| 7 | **Estado global mutable:** `Datos` es una lista estática compartida entre instancias. | El estado tiene ciclo de vida no controlado, puede contaminar ejecuciones y no ofrece coordinación para accesos concurrentes. | Alta |
| 8 | **Responsabilidades mezcladas:** la misma clase maneja almacenamiento, reglas, fecha del sistema y formatos de búsqueda/reporte. | La cohesión es baja; cambios de almacenamiento o presentación fuerzan cambios en la misma lógica. | Media |

## Priorización

Se priorizan primero la representación posicional, el estado global, el manejo de errores y el acoplamiento del método, porque afectan la integridad de los datos, la capacidad de prueba y el riesgo de regresión. La separación de responsabilidades y la extracción de consultas reducen después el costo de evolución.

La línea base solo valida asunto y descripción contra cadenas vacías; no valida, por ejemplo, cadenas compuestas por espacios. Además, `DateTime.Now` se obtiene dentro de la operación, lo que dificulta pruebas deterministas. Estos aspectos refuerzan la necesidad de validación explícita y de sustituir dependencias externas en pruebas.

## Dirección de mejora

Para conservar el alcance del prototipo sin introducir una base de datos ni un framework adicional, el refactoring separa dominio, casos de uso, consultas, puertos y adaptador en memoria. También reemplaza arreglos y literales por tipos expresivos, divide las operaciones, informa errores de manera explícita y abstrae el reloj. La persistencia en memoria sigue siendo una limitación conocida: no es durable y no debe presentarse como lista para producción.

## Evidencia

- Línea base analizada: [CodigoAntes.cs](../codigo_antes/CodigoAntes.cs).
- Código refactorizado: `src/Solicitudes.Core`.
- Pruebas automatizadas y sus límites: [pruebas-unitarias.txt](evidencias/pruebas-unitarias.txt).
