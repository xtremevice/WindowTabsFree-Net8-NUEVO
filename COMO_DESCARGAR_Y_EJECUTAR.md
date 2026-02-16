# 📥 Cómo Descargar y Ejecutar WindowTabsFree

Guía completa con los comandos exactos para descargar la última versión y ejecutar WindowTabsFree en tu sistema.

---

## 🎯 Opción 1: Descargar y Ejecutar desde el Código Fuente (Recomendado para Desarrollo)

### Paso 1: Clonar el Repositorio

```bash
# Clonar el repositorio desde GitHub
git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git

# Entrar al directorio del proyecto
cd WindowTabsFree-Net8-NUEVO
```

### Paso 2: Ejecutar Directamente

```bash
# Ejecutar la aplicación (requiere .NET 8 SDK)
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

¡Eso es todo! La aplicación se abrirá en una ventana.

---

## 🎯 Opción 2: Descargar Ejecutable Pre-compilado (Próximamente)

> **Nota:** Los ejecutables pre-compilados estarán disponibles en la sección de [Releases de GitHub](https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/releases).

Cuando estén disponibles:

### Linux (x64)
```bash
# Descargar el ejecutable
wget https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/releases/download/vX.X.X/WindowTabsFree-linux-x64.tar.gz

# Extraer
tar -xzf WindowTabsFree-linux-x64.tar.gz

# Dar permisos de ejecución
chmod +x WindowTabsFree.UI

# Ejecutar
./WindowTabsFree.UI
```

### Windows (x64)
```powershell
# Descargar desde: https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/releases
# Extraer el ZIP descargado
# Ejecutar:
.\WindowTabsFree.UI.exe
```

### macOS (Apple Silicon - M1/M2/M3)
```bash
# Descargar desde: https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/releases
# Extraer el archivo
chmod +x WindowTabsFree.UI

# Ejecutar
./WindowTabsFree.UI
```

---

## 🎯 Opción 3: Compilar desde el Código Fuente

### Prerequisitos
Antes de compilar, asegúrate de tener instalado .NET 8 SDK:

```bash
# Verificar si tienes .NET 8 instalado
dotnet --version
```

Si ves una versión 8.x.x, estás listo. Si no, descarga .NET 8 SDK desde:
- https://dotnet.microsoft.com/download/dotnet/8.0

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git
cd WindowTabsFree-Net8-NUEVO
```

### Paso 2: Compilar para tu Plataforma

#### **Linux (x64)**
```bash
# Usar el script de compilación incluido
chmod +x publish-linux.sh
./publish-linux.sh

# Ejecutar el ejecutable compilado
./publish/linux-x64/WindowTabsFree.UI
```

O manualmente:
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/linux-x64

# Ejecutar
chmod +x ./publish/linux-x64/WindowTabsFree.UI
./publish/linux-x64/WindowTabsFree.UI
```

#### **Windows (x64)**
```powershell
# Compilar
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish\win-x64

# Ejecutar
.\publish\win-x64\WindowTabsFree.UI.exe
```

#### **macOS (Apple Silicon)**
```bash
# Usar el script de compilación incluido
chmod +x publish-macos-arm64.sh
./publish-macos-arm64.sh

# Ejecutar
./publish/macos-arm64/WindowTabsFree.UI
```

O manualmente:
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/macos-arm64

# Ejecutar
chmod +x ./publish/macos-arm64/WindowTabsFree.UI
./publish/macos-arm64/WindowTabsFree.UI
```

#### **macOS (Intel)**
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r osx-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/macos-x64

# Ejecutar
chmod +x ./publish/macos-x64/WindowTabsFree.UI
./publish/macos-x64/WindowTabsFree.UI
```

---

## 📋 Requisitos del Sistema

### Todos los Sistemas
- **.NET 8 Runtime** (o SDK para compilación)
- Memoria RAM: 100 MB mínimo
- Espacio en disco: 200 MB

### Linux
- **Servidor de pantalla X11** (no funciona en Wayland puro)
- **libX11.so.6** instalado

Instalar dependencias:
```bash
# Ubuntu/Debian
sudo apt-get install libx11-6

# Fedora/RHEL/CentOS
sudo dnf install libX11

# Arch Linux
sudo pacman -S libx11
```

Verificar X11:
```bash
echo $DISPLAY
# Debe mostrar algo como :0 o :1
```

### Windows
- **Windows 7 o superior**
- No requiere dependencias adicionales

### macOS
- **macOS 10.15 (Catalina) o superior**
- **Permisos de Accesibilidad** (se solicitarán al ejecutar)

Configurar permisos:
1. Abrir **Preferencias del Sistema** → **Seguridad y Privacidad** → **Privacidad** → **Accesibilidad**
2. Agregar WindowTabsFree.UI a la lista
3. Habilitar el checkbox

---

## 🚀 Comandos Rápidos - Resumen

### Para Empezar Rápidamente (con .NET 8 SDK instalado)

```bash
# 1. Clonar
git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git

# 2. Entrar al directorio
cd WindowTabsFree-Net8-NUEVO

# 3. Ejecutar
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Para Actualizar a la Última Versión

```bash
# Desde el directorio del proyecto
cd WindowTabsFree-Net8-NUEVO

# Obtener los últimos cambios
git pull origin main

# Ejecutar
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Para Compilar un Ejecutable

```bash
# Linux
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI

# macOS
./publish-macos-arm64.sh
./publish/macos-arm64/WindowTabsFree.UI

# Windows (PowerShell)
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained -o .\publish\win-x64
.\publish\win-x64\WindowTabsFree.UI.exe
```

---

## 🛠️ Solución de Problemas

### Error: "No se encuentra el SDK de .NET"

**Solución:** Instala .NET 8 SDK desde https://dotnet.microsoft.com/download/dotnet/8.0

Verificar instalación:
```bash
dotnet --version
# Debe mostrar 8.0.x
```

### Error en Linux: "X11 display not available"

**Causa:** No estás ejecutando desde un entorno gráfico o X11 no está configurado.

**Solución:**
```bash
# Verificar que DISPLAY esté configurado
echo $DISPLAY

# Si está vacío, necesitas estar en una sesión gráfica con X11
# No funcionará en SSH sin X11 forwarding
```

### Error en Linux: "libX11.so.6 no encontrado"

**Solución:** Instala la librería X11:
```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install libx11-6

# Fedora/RHEL
sudo dnf install libX11
```

### Error en macOS: "Cannot control windows"

**Causa:** Faltan permisos de accesibilidad.

**Solución:**
1. Abrir **Preferencias del Sistema** → **Seguridad y Privacidad**
2. Ir a **Privacidad** → **Accesibilidad**
3. Hacer clic en el candado para hacer cambios
4. Agregar WindowTabsFree.UI a la lista
5. Habilitar el checkbox
6. Reiniciar la aplicación

### Error: "Git no reconocido como comando"

**Solución:** Instala Git:
- **Windows:** https://git-scm.com/download/win
- **Linux:** `sudo apt-get install git` (Ubuntu/Debian) o `sudo dnf install git` (Fedora)
- **macOS:** `xcode-select --install`

### Error al Compilar: "Project not found"

**Causa:** No estás en el directorio correcto del proyecto.

**Solución:**
```bash
# Verificar dónde estás
pwd

# Navegar al directorio del proyecto
cd WindowTabsFree-Net8-NUEVO

# Verificar que ves la carpeta src/
ls -la
```

---

## 📚 Documentación Adicional

Después de descargar y ejecutar, consulta:

- **[QUICKSTART_ES.md](QUICKSTART_ES.md)** - Guía rápida de uso
- **[README.md](README.md)** - Documentación completa en inglés
- **[GUIA_USO_MEJORADA.md](GUIA_USO_MEJORADA.md)** - Características avanzadas
- **[GUIA_AGRUPACION.md](GUIA_AGRUPACION.md)** - Sistema de agrupación de ventanas

---

## ❓ Preguntas Frecuentes

### ¿Necesito tener Visual Studio instalado?

**No.** Solo necesitas .NET 8 SDK. Puedes usar cualquier editor de código o solo la línea de comandos.

### ¿Puedo ejecutar sin instalar .NET?

**Sí**, si descargas un ejecutable pre-compilado con `--self-contained true` (incluye .NET). Pero para ejecutar desde el código fuente, necesitas .NET 8 SDK.

### ¿Cómo sé si tengo X11 o Wayland en Linux?

```bash
echo $XDG_SESSION_TYPE
# Mostrará "x11" o "wayland"

# WindowTabsFree requiere X11
# Si usas Wayland, necesitas configurar X11 compatibility (XWayland)
```

### ¿Funciona en Linux ARM (Raspberry Pi)?

Sí, pero necesitas compilar para `linux-arm64`:
```bash
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r linux-arm64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o ./publish/linux-arm64
```

### ¿Puedo usar un IDE?

**Sí**, puedes abrir el proyecto en:
- **Visual Studio 2022** (Windows/Mac)
- **Visual Studio Code** con C# extension
- **JetBrains Rider**
- Cualquier IDE que soporte .NET

---

## 🔄 Mantener la Aplicación Actualizada

### Ver la versión actual instalada

```bash
cd WindowTabsFree-Net8-NUEVO
git log -1 --oneline
```

### Actualizar a la última versión

```bash
# Desde el directorio del proyecto
cd WindowTabsFree-Net8-NUEVO

# Descargar los últimos cambios
git fetch origin

# Actualizar tu copia local
git pull origin main

# Recompilar (opcional, pero recomendado)
dotnet build
```

### Ver qué cambió desde tu versión

```bash
git log --oneline --since="1 week ago"
```

---

## 💡 Consejos Útiles

### 1. Crear un Alias para Ejecutar Fácilmente

**Linux/macOS:**
```bash
# Agregar al archivo ~/.bashrc o ~/.zshrc
echo 'alias windowtabs="cd ~/WindowTabsFree-Net8-NUEVO && dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj"' >> ~/.bashrc

# Recargar
source ~/.bashrc

# Ahora puedes ejecutar desde cualquier lugar:
windowtabs
```

**Windows (PowerShell):**
```powershell
# Agregar al perfil de PowerShell
echo 'function windowtabs { cd C:\WindowTabsFree-Net8-NUEVO; dotnet run --project src\WindowTabsFree.UI\WindowTabsFree.UI.csproj }' >> $PROFILE

# Recargar
. $PROFILE

# Ejecutar:
windowtabs
```

### 2. Compilar una Vez, Ejecutar Muchas Veces

```bash
# Compilar una vez
dotnet build src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Ejecutar rápidamente (sin recompilar)
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj --no-build
```

### 3. Modo de Desarrollo con Auto-Recarga

```bash
# Observar cambios y recompilar automáticamente
dotnet watch run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

## 📞 Soporte

Si tienes problemas:

1. **Revisa la documentación:**
   - [QUICKSTART_ES.md](QUICKSTART_ES.md)
   - [README.md](README.md)
   - Este archivo (COMO_DESCARGAR_Y_EJECUTAR.md)

2. **Verifica los requisitos del sistema** en la sección de Requisitos arriba

3. **Consulta los problemas conocidos** en la sección de Solución de Problemas

4. **Crea un issue en GitHub:**
   - https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/issues

---

## ✨ Resumen de Un Vistazo

| Acción | Comando |
|--------|---------|
| **Clonar repo** | `git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git` |
| **Entrar al directorio** | `cd WindowTabsFree-Net8-NUEVO` |
| **Ejecutar (desarrollo)** | `dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj` |
| **Compilar Linux** | `./publish-linux.sh` |
| **Compilar macOS** | `./publish-macos-arm64.sh` |
| **Compilar Windows** | `dotnet publish ... -r win-x64 --self-contained -o .\publish\win-x64` |
| **Actualizar** | `git pull origin main` |
| **Ver versión .NET** | `dotnet --version` |

---

**¡Listo para empezar! 🚀**

Usa los comandos de arriba según tu sistema operativo y necesidades.
