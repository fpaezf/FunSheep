# VB.NET FunSheep desktop pet (Clon de eSheep)

<img width="100%" src="https://i.postimg.cc/XJ1WhwG4/2.png">

<img alt="Windows" src="https://img.shields.io/badge/-Windows-0078D6?style=flat&logo=windows&logoColor=white"/> <img alt="NET" src="https://img.shields.io/badge/-Visual%20Basic-blue?style=flat&logo=.net&logoColor=white"/>

Translate this page to english: https://bit.ly/3I2wRPp

Esta aplicación es un clon escrito en Visual Basic.NET del programa <b>eSheep</b> del autor <b>AdrianoTiger</b>: https://github.com/Adrianotiger/desktopPet

Los <b>desktop pets</b> o <b>Screenmates</b> fueron muy populares en la época de Windows95/98/XP pero debido a la inclusion de software malicioso por parte de terceros y la saturación de aplicaciones de este tipo, hicieron que al final la gente perdiera el interés y cayeran en desuso.

Esta aplicación es un pequeño tributo a los screenmates de antaño y a sus creadores.

# Funcionamiento interno
- Todas las animaciones son arrays de imágenes (Bitmaps) que se guardan como recursos incrustados en un archivo .resx.
- Al ejecutar el programa, se inicia un reloj (Timer) y con cada tick del reloj se calcula la animación y las imágenes a mostrar.
- Cada cierto tiempo la oveja ejecuta animaciones aleatorias como dormir, hacer pis, correr... controladas mediante un reloj (timer).
- La oveja detecta los bordes de la pantalla.
- La oveja detecta ventanas abiertas y se posa sobre ellas.

# Animaciones implementadas
- Caer al vacío
- Estrellarse contra el suelo
- Caminar sobre la barra de tareas
- Caminar sobre ventanas abiertas
- 2 formas distintas de dormirse
- Ponerse a 2 patas
- Correr
- Hacer pis
- Estrellarse contra los bordes de la pantalla
- Mas animaciones por implementar*

# Sonidos
- La oveja reproduce balidos aleatoriamente (se puede desactivar).

# APIs de Windows implementadas
- GetWindowRectangle() <-- Substituye a GetWindowRect() porque obtiene el tamaño de la ventana sin sombras en Win10.<br>
- DwmGetWindowAttribute()<br>
- GetWindow()<br>
- GetWindowText()<br>
- GetTopWindow()<br>
- IsWindowVisible()<br>
- EnumWindows()<br>
- EnumWindowsDelegate()<br>

# Fallos conocidos
- La colisión contra ventanas no es del todo correcta.
- Faltan depurar algunas animaciones.
- Al matar todas las ovejas alguna desaparece sin la animación correspondiente.

# Agradecimientos
- <b>AdrianoTiger</b> por el código fuente de eSheep de donde he sacado algún recorte de código: https://github.com/Adrianotiger<br>
- <b>StackOverflow</b> por la ayuda y códigos fuente: https://stackoverflow.com<br>
