# 🍎 Guía Completa: WindowTabsFree en Mac Apple Silicon (M1/M2/M3)

Esta guía te llevará paso a paso desde verificar que tienes todo lo necesario hasta ejecutar WindowTabsFree en tu Mac con chip Apple Silicon.

---

## 📋 Tabla de Contenidos

1. [Verificar Requisitos](#1-verificar-requisitos)
2. [Instalar Dependencias](#2-instalar-dependencias)
3. [Descargar el Proyecto](#3-descargar-el-proyecto)
4. [Configurar Permisos](#4-configurar-permisos)
5. [Ejecutar la Aplicación](#5-ejecutar-la-aplicación)
6. [Solución de Problemas](#6-solución-de-problemas)

---

## 1. Verificar Requisitos

### 🚀 Método Rápido: Script Automático de Verificación

Si quieres verificar todo de una vez, ejecuta:

```bash
cd ~/WindowTabsFree  # O donde tengas el proyecto
./check-macos-requirements.sh
```

Este script verificará automáticamente:
- ✅ Versión de macOS
- ✅ Arquitectura (Apple Silicon vs Intel)
- ✅ .NET 8 SDK instalado
- ✅ Git instalado
- ✅ Ubicación del proyecto
- ✅ Xcode Command Line Tools

**O verifica manualmente siguiendo los pasos a continuación:**

---

### Paso 1.1: Confirmar que tienes Apple Silicon

Abre **Terminal** (Aplicaciones → Utilidades → Terminal) y ejecuta:

```bash
uname -m
```

**Resultado esperado:**
```
arm64
```

✅ Si ves `arm64` → Tienes Apple Silicon (M1/M2/M3)  
❌ Si ves `x86_64` → Tienes Intel (sigue la guía normal de macOS)

### Paso 1.2: Verificar versión de macOS

```bash
sw_vers
```

**Resultado esperado:**
```
ProductName:    macOS
ProductVersion: 13.0 o superior
BuildVersion:   ...
```

✅ Necesitas **macOS 10.15 (Catalina) o superior**

### Paso 1.3: Verificar si .NET 8 está instalado

```bash
dotnet --version
```

**Resultado esperado:**
```
8.0.xxx
```

✅ Si ves versión 8.0.x → .NET 8 está instalado  
❌ Si ves error "command not found" → Necesitas instalar .NET 8 (ver sección 2)

### Paso 1.4: Verificar si git está instalado

```bash
git --version
```

**Resultado esperado:**
```
git version 2.x.x
```

✅ Si ves una versión → Git está instalado  
❌ Si no está instalado → macOS te pedirá instalarlo automáticamente

---

## 2. Instalar Dependencias

### Paso 2.1: Instalar .NET 8 SDK para Apple Silicon

**Opción A: Descargar desde el sitio oficial (Recomendado)**

1. Ve a: https://dotnet.microsoft.com/download/dotnet/8.0
2. En la sección **".NET SDK"**, busca **"macOS Arm64"**
3. Descarga el instalador (archivo `.pkg`)
4. Ejecuta el instalador y sigue las instrucciones
5. Cierra y vuelve a abrir Terminal

**Opción B: Usar Homebrew**

Si tienes Homebrew instalado:

```bash
brew install --cask dotnet-sdk
```

**Verificar instalación:**

```bash
dotnet --version
dotnet --list-runtimes
dotnet --list-sdks
```

Deberías ver algo como:
```
8.0.xxx
Microsoft.AspNetCore.App 8.0.x [/usr/local/share/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.NETCore.App 8.0.x [/usr/local/share/dotnet/shared/Microsoft.NETCore.App]
8.0.xxx [/usr/local/share/dotnet/sdk]
```

### Paso 2.2: Instalar Git (si no lo tienes)

Git generalmente viene con Xcode Command Line Tools:

```bash
xcode-select --install
```

O con Homebrew:

```bash
brew install git
```

---

## 3. Descargar el Proyecto

### Paso 3.1: Elegir ubicación

Decide dónde guardar el proyecto. Recomendamos tu carpeta de usuario:

```bash
cd ~
```

O crear una carpeta de proyectos:

```bash
mkdir -p ~/Projects
cd ~/Projects
```

### Paso 3.2: Clonar el repositorio

```bash
git clone https://github.com/xtremevice/WindowTabsFree.git
```

**Resultado esperado:**
```
Cloning into 'WindowTabsFree'...
remote: Enumerating objects: xxx, done.
remote: Counting objects: 100% (xxx/xxx), done.
remote: Compressing objects: 100% (xxx/xxx), done.
Receiving objects: 100% (xxx/xxx), x.xx MiB | x.xx MiB/s, done.
Resolving deltas: 100% (xxx/xxx), done.
```

### Paso 3.3: Cambiar al branch correcto

```bash
cd WindowTabsFree
git checkout copilot/implement-window-service-functionality
```

**Resultado esperado:**
```
Branch 'copilot/implement-window-service-functionality' set up to track remote branch...
Switched to a new branch 'copilot/implement-window-service-functionality'
```

### Paso 3.4: Verificar que tienes los archivos correctos

```bash
ls -la src/
```

Deberías ver:
```
WindowTabsFree.Common/
WindowTabsFree.Core/
WindowTabsFree.Services/
WindowTabsFree.TestConsole/
WindowTabsFree.UI/
```

```bash
ls -la src/WindowTabsFree.Services/macOS/
```

Deberías ver:
```
MacOSWindowService.cs
```

---

## 4. Configurar Permisos

WindowTabsFree necesita permisos de **Accesibilidad** para controlar otras ventanas en macOS.

### Paso 4.1: Preparar para solicitud de permisos

Los permisos se solicitarán la primera vez que ejecutes la aplicación y trates de usar funciones de control de ventanas.

**IMPORTANTE:** No necesitas configurar permisos ANTES de ejecutar por primera vez. macOS te lo pedirá automáticamente.

### Paso 4.2: Cómo otorgar permisos cuando se soliciten

Cuando ejecutes la aplicación y trates de controlar ventanas, verás un diálogo:

```
"Terminal" would like to control "System Events"
```

1. Haz clic en **"OK"**
2. macOS te redirigirá a **Preferencias del Sistema**
3. Ve a: **Seguridad y Privacidad** → **Privacidad** → **Accesibilidad**
4. Desbloquea haciendo clic en el candado 🔒 (abajo izquierda)
5. Marca la casilla junto a **Terminal** (o la aplicación que estés usando)
6. Cierra Preferencias del Sistema

**Nota:** Solo necesitas hacer esto UNA VEZ. El permiso quedará guardado.

---

## 5. Ejecutar la Aplicación

### 🚀 MÉTODO 1: Ejecutar Directamente (Desarrollo)

Este es el método más rápido para probar:

```bash
cd ~/WindowTabsFree
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

**Primera vez puede tardar más** porque descarga dependencias:
```
Determining projects to restore...
Restored /Users/tu-usuario/WindowTabsFree/src/WindowTabsFree.Common/WindowTabsFree.Common.csproj
Restored /Users/tu-usuario/WindowTabsFree/src/WindowTabsFree.Core/WindowTabsFree.Core.csproj
...
Building...
```

**Cuando esté listo verás:**
```
info: Avalonia[0]
      Avalonia initialized
```

Y se abrirá la ventana de la aplicación! 🎉

### 🚀 MÉTODO 2: Compilar Ejecutable Standalone (Producción)

Si quieres crear un ejecutable independiente optimizado para Apple Silicon:

#### Opción A: Usar el Script Automático (Recomendado)

El método más fácil es usar el script de compilación incluido:

```bash
cd ~/WindowTabsFree
./publish-macos-arm64.sh
```

Este script:
- ✅ Verifica que estás en Apple Silicon
- ✅ Compila optimizado para ARM64
- ✅ Crea ejecutable autocontenido
- ✅ Muestra información del resultado
- ✅ Proporciona instrucciones de ejecución

Cuando termine verás:
```
✅ ¡Compilación exitosa!
📊 Información del ejecutable:
   📁 Ubicación: ./publish/macos-arm64/WindowTabsFree.UI
   💾 Tamaño total: ~90MB
   🏗️  Arquitectura: ARM64 (Apple Silicon)
```

#### Opción B: Compilación Manual

Si prefieres compilar manualmente:

#### Paso 5.1: Crear ejecutable

```bash
cd ~/WindowTabsFree
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
  -r osx-arm64 \
  --self-contained \
  -c Release \
  -o publish/macos-arm64
```

Esto tomará unos minutos y creará un ejecutable de ~90-100MB.

**Resultado esperado:**
```
WindowTabsFree.UI -> /Users/tu-usuario/WindowTabsFree/publish/macos-arm64/WindowTabsFree.UI.dll
WindowTabsFree.UI -> /Users/tu-usuario/WindowTabsFree/publish/macos-arm64/
```

#### Paso 5.2: Ejecutar el ejecutable

```bash
cd ~/WindowTabsFree
./publish/macos-arm64/WindowTabsFree.UI
```

O desde Finder:
1. Abre Finder
2. Ve a `WindowTabsFree/publish/macos-arm64/`
3. Doble clic en `WindowTabsFree.UI`

**Si macOS bloquea la ejecución:**
1. Ve a **Preferencias del Sistema** → **Seguridad y Privacidad** → **General**
2. Verás un mensaje sobre `WindowTabsFree.UI`
3. Haz clic en **"Abrir de todas formas"**

### 🚀 MÉTODO 3: Comando Todo-en-Uno

Para actualizar y ejecutar la última versión en un solo comando:

```bash
cd ~/WindowTabsFree && \
git pull origin copilot/implement-window-service-functionality && \
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

## 6. Usar la Aplicación

### Interfaz de Usuario

Cuando se abra la aplicación verás:

**Panel Izquierdo - Grupos:**
- Lista de grupos de ventanas
- Botón "Auto-Group by App" (verde)
- Botón "Remove Auto-Groups" (naranja)
- Botones para crear/eliminar grupos

**Panel Derecho - Ventanas Activas:**
- Lista de todas las ventanas abiertas
- Nombre de la aplicación (ProcessName)
- Estado (Minimized/Maximized)
- Botones: Focus, Minimize, Maximize, Restore, Close
- Botón "Add to Group"

### Funcionalidades Principales

#### 1. Ver todas las ventanas abiertas
- Las ventanas se actualizan automáticamente cada 2 segundos
- Muestra el nombre de la aplicación para cada ventana

#### 2. Agrupar ventanas automáticamente
1. Haz clic en **"Auto-Group by App"**
2. Se crearán grupos automáticamente para aplicaciones con múltiples ventanas
3. Ejemplo: Si tienes 3 ventanas de Chrome, se creará un grupo "Chrome" con las 3

#### 3. Navegar entre pestañas de un grupo
1. Selecciona un grupo en el panel izquierdo
2. Usa los botones **"◄ Prev Tab"** y **"Next Tab ►"**
3. La ventana activa se marca en verde

#### 4. Controlar ventanas individualmente
- **Focus**: Trae la ventana al frente
- **Minimize**: Minimiza la ventana
- **Maximize**: Pone la ventana en pantalla completa
- **Restore**: Restaura el tamaño normal
- **Close**: Cierra la ventana

#### 5. Crear grupos manualmente
1. Haz clic en "New Group" en el panel izquierdo
2. Escribe un nombre para el grupo
3. Selecciona ventanas y haz clic en "Add to Group"

---

## 7. Solución de Problemas

### ❌ Error: "command not found: dotnet"

**Problema:** .NET 8 no está instalado o no está en el PATH.

**Solución:**
```bash
# Verificar si está instalado pero no en PATH
/usr/local/share/dotnet/dotnet --version

# Si funciona, agregar a PATH
echo 'export PATH="/usr/local/share/dotnet:$PATH"' >> ~/.zshrc
source ~/.zshrc
```

O reinstalar .NET 8 desde https://dotnet.microsoft.com/download

### ❌ Error: "fatal: not a git repository"

**Problema:** No estás en el directorio correcto.

**Solución:**
```bash
# Buscar el directorio
find ~ -name "WindowTabsFree" -type d 2>/dev/null

# Navegar al directorio encontrado
cd /ruta/encontrada/WindowTabsFree
```

### ❌ Error: "Project file does not exist"

**Problema:** No estás en el branch correcto o falta el código.

**Solución:**
```bash
cd ~/WindowTabsFree
git checkout copilot/implement-window-service-functionality
git pull origin copilot/implement-window-service-functionality
```

### ❌ Error: Las funciones de control no funcionan

**Problema:** No has otorgado permisos de Accesibilidad.

**Solución:**
1. Ve a **Preferencias del Sistema** → **Seguridad y Privacidad**
2. Haz clic en **Privacidad**
3. Selecciona **Accesibilidad** en la lista izquierda
4. Desbloquea con el candado 🔒
5. Marca **Terminal** (o la app que uses para ejecutar)
6. Reinicia la aplicación

### ❌ La aplicación no muestra ventanas

**Problema:** Puede ser normal si no hay muchas ventanas abiertas, o las ventanas están en otros Spaces.

**Solución:**
```bash
# Verificar que la aplicación está leyendo ventanas
# En el terminal donde ejecutaste la app, busca logs
```

Asegúrate de tener algunas aplicaciones abiertas (Safari, Chrome, Terminal, etc.)

### ❌ Error de compilación: "platform not supported"

**Problema:** Ejecutaste un comando para Intel en vez de ARM64.

**Solución:**
```bash
# Para Apple Silicon usa:
dotnet publish -r osx-arm64 --self-contained

# NO uses:
dotnet publish -r osx-x64 --self-contained  # Esto es para Intel
```

### ❌ Error: "dyld: Library not loaded"

**Problema:** Falta alguna biblioteca del sistema.

**Solución:**
```bash
# Instalar Xcode Command Line Tools
xcode-select --install

# Verificar instalación
xcode-select -p
```

### ❌ La aplicación se cierra inmediatamente

**Problema:** Puede haber un error de ejecución.

**Solución:**
```bash
# Ejecutar con logs detallados
DOTNET_CLI_TELEMETRY_OPTOUT=1 \
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj --verbosity detailed
```

---

## 8. Características Específicas de macOS

### Full-Screen vs Maximize

- **Maximize** en macOS usa el modo **Full-Screen nativo**
- Esto crea un nuevo Space para la aplicación
- Usa gestos de trackpad para cambiar entre Spaces

### Mission Control y Spaces

- WindowTabsFree enumera ventanas en **todos los Spaces**
- Hacer focus en una ventana cambia al Space donde está
- Esto es comportamiento estándar de macOS

### Dock Integration

- Las ventanas minimizadas aparecen en el Dock
- WindowTabsFree puede minimizar/restaurar desde el Dock
- Los grupos persisten incluso si las apps están minimizadas

---

## 9. Próximos Pasos

Una vez que tengas la aplicación funcionando:

1. **Lee la guía de agrupación:** [GUIA_AGRUPACION.md](GUIA_AGRUPACION.md)
2. **Explora las características:** Prueba agrupar diferentes aplicaciones
3. **Configura tus preferencias:** Los settings se guardan automáticamente
4. **Reporta problemas:** Si encuentras bugs, repórtalos en GitHub

---

## 10. Resumen de Comandos Rápidos

### 🚀 Super Rápido (Todo en Uno)

```bash
# Verificar requisitos + Clonar + Ejecutar
cd ~/Projects && \
git clone https://github.com/xtremevice/WindowTabsFree.git && \
cd WindowTabsFree && \
git checkout copilot/implement-window-service-functionality && \
./check-macos-requirements.sh && \
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

### Primera Vez (Instalación Completa)

```bash
# 1. Verificar requisitos (NUEVO - Script automático)
cd ~/WindowTabsFree
./check-macos-requirements.sh

# 1b. O verificar manualmente
uname -m                    # Debe decir: arm64
sw_vers                     # Debe ser macOS 10.15+
dotnet --version            # Debe decir: 8.0.x

# 2. Clonar proyecto (si no lo tienes)
cd ~/Projects  # o donde prefieras
git clone https://github.com/xtremevice/WindowTabsFree.git
cd WindowTabsFree
git checkout copilot/implement-window-service-functionality

# 3. Ejecutar
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Ejecución Posterior (Ya instalado)

```bash
# Opción 1: Simple
cd ~/WindowTabsFree
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Opción 2: Con actualización
cd ~/WindowTabsFree
git pull origin copilot/implement-window-service-functionality
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Opción 3: Todo en uno
cd ~/WindowTabsFree && git pull origin copilot/implement-window-service-functionality && dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Compilar Ejecutable Standalone

```bash
# Opción 1: Con script automático (RECOMENDADO)
cd ~/WindowTabsFree
./publish-macos-arm64.sh

# Opción 2: Manual
cd ~/WindowTabsFree
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
  -r osx-arm64 \
  --self-contained \
  -c Release \
  -o publish/macos-arm64

# Ejecutar
./publish/macos-arm64/WindowTabsFree.UI
```

---

## 📞 Ayuda Adicional

- **Guía macOS General:** [MACOS_GUIDE.md](MACOS_GUIDE.md)
- **Guía de Agrupación:** [GUIA_AGRUPACION.md](GUIA_AGRUPACION.md)
- **Guía Rápida:** [QUICKSTART_ES.md](QUICKSTART_ES.md)
- **README Principal:** [README_NET8.md](README_NET8.md)

---

## ✅ Checklist Final

Antes de reportar un problema, verifica:

- [ ] Tengo macOS 10.15 o superior
- [ ] Tengo un Mac con Apple Silicon (M1/M2/M3)
- [ ] .NET 8 SDK está instalado (`dotnet --version` funciona)
- [ ] Estoy en el directorio correcto (`pwd` muestra WindowTabsFree)
- [ ] Estoy en el branch correcto (`git branch` muestra copilot/implement-window-service-functionality)
- [ ] He otorgado permisos de Accesibilidad a Terminal
- [ ] Tengo algunas aplicaciones abiertas para probar
- [ ] El proyecto compila sin errores (`dotnet build` funciona)

---

**¡Disfruta usando WindowTabsFree en tu Mac con Apple Silicon! 🚀**
