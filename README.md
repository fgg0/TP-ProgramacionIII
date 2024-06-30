Trabajo de Programación III: 'FarewellFest' - Grupo 6

Integrantes del grupo:
- Aguirre, Agustina
- Graff, Francisco
- Lucas, Natacha
- Martinez, Mariano

Instrucciones para iniciar el sistema:
- Clonar el repositorio <link>
- Abrir el proyecto en Visual Studio y configurar el inicio múltiple para los proyectos GestorEventos.WebUsuario y GestorEventos.WebAdmin (no recomendamos correr uno solo debido a que comparten archivos para simplificar la estructura).
- En los servicios cambiar el valor _connectionString a la base de datos local.
El sistema ya estaría listo para su uso.

Explicación general de la solución:
Está desarrollada con el Framework de Microsoft .NET y consta de cuatro proyectos:
• GestorEventos.Api: API del sistema.
• GestorEventos.Servicios: Servicio de acceso a la base de datos.
• GestorEventos.WebAdmin: Aplicación web privada para administrar el servicio.
• GestorEventos.WebUsuario: Aplicación web pública para los usuarios y fines comerciales.

- Base de datos: Se creó una propia. Se incluye el archivo para utilizar la misma <link>. 
- Método de acceso a la base de datos: Empleamos Dapper para realizar las peticiones.
- Disposición de los datos a la web: Se utilizó una API REST y dos Web App MVC (Modelo - Vista - Controlador), una dirigida para los usuarios del servicio y otra para el administrador de los eventos.

Observaciones:
- Nuestra metodología de trabajo fue utilizar la misma máquina para desarrollar el proyecto, a través de los distintos aportes realizados por los integrantes mediante la plataforma Meet u otro servicio de comunicación. Por lo tanto, el commit fue subido directamente desde Git, razón por la cuál no aparecen las colaboraciones de los distintos miembros del grupo. 
Se fueron realizando distintas modificaciones para dejar el proyecto en óptimas condiciones. Por esto, figuran commits fuera de la fecha establecida.
