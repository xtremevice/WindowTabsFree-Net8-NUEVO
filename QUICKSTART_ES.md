# Guía Rápida - Cómo ejecutar WindowTabsFree desde la terminal

Esta guía te muestra cómo ejecutar WindowTabsFree directamente desde tu terminal.

## 📥 ¿Necesitas descargar el proyecto primero?

**Si aún no tienes el proyecto descargado**, consulta la guía completa de descarga:
- **[COMO_DESCARGAR_Y_EJECUTAR.md](COMO_DESCARGAR_Y_EJECUTAR.md)** ⭐

Comandos rápidos:
```bash
# Clonar desde GitHub
git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git
cd WindowTabsFree-Net8-NUEVO
```

---

## ⚠️ IMPORTANTE: Debes estar en el directorio del proyecto

Todos los comandos asumen que estás en el directorio `WindowTabsFree`. Verifica primero:

```bash
pwd                    # Ver dónde estás
cd WindowTabsFree     # Navegar al proyecto (si es necesario)
git status            # Verificar que es un repositorio git
ls src/               # Deberías ver los proyectos
```

Si ves errores como:
- `fatal: not a git repository` 
- `Project file does not exist`

→ **No estás en el directorio correcto.** Usa `cd WindowTabsFree` primero.

---

## Opción 1: Ejecutar en Modo Desarrollo (Más Rápido)

### Prerrequisitos
- Tener instalado .NET 8 SDK
- Estar en el directorio del proyecto (ver sección IMPORTANTE arriba)

### Pasos

**Ejecuta la aplicación directamente:**
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

¡Eso es todo! La aplicación se abrirá en una ventana.

---

## Opción 2: Compilar y Ejecutar (Para Distribución)

### En Linux

1. **Compila el ejecutable standalone:**
```bash
chmod +x publish-linux.sh
./publish-linux.sh
```

2. **Ejecuta el ejecutable:**
```bash
./publish/linux-x64/WindowTabsFree.UI
```

### En Windows

1. **Compila el ejecutable standalone:**
```powershell
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64
```

2. **Ejecuta el ejecutable:**
```powershell
.\publish\win-x64\WindowTabsFree.UI.exe
```

---

## Opción 3: Solo Compilar (Sin Ejecutar)

```bash
dotnet build WindowTabsFreeNet.sln
```

Después puedes ejecutar manualmente:
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

## Verificar que .NET 8 está instalado

```bash
dotnet --version
```

Deberías ver una versión 8.x.x. Si no tienes .NET 8, descárgalo de:
- https://dotnet.microsoft.com/download/dotnet/8.0

---

## Probar la Consola de Pruebas (Opcional)

Si solo quieres probar que todo funciona sin abrir la UI:

```bash
dotnet run --project src/WindowTabsFree.TestConsole/WindowTabsFree.TestConsole.csproj
```

---

## Solución de Problemas

### Error: "No se encuentra el SDK de .NET"
**Solución:** Instala .NET 8 SDK desde https://dotnet.microsoft.com/download

### Error en Linux: "X11 display not available"
**Solución:** Asegúrate de estar ejecutando desde un entorno gráfico con X11:
```bash
echo $DISPLAY
```
Si está vacío, necesitas estar en una sesión gráfica.

### Error: "libX11.so.6 no encontrado" (Linux)
**Solución:** Instala la librería X11:
```bash
# Ubuntu/Debian
sudo apt-get install libx11-6

# Fedora/RHEL
sudo dnf install libX11
```

---

## Resumen de Comandos Más Usados

```bash
# Ejecutar directamente (desarrollo)
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Compilar ejecutable para Linux
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI

# Solo compilar el proyecto
dotnet build WindowTabsFreeNet.sln

# Ver versión de .NET
dotnet --version
```

---

## Más Información

- **README completo:** README_NET8.md
- **Documentación técnica:** IMPLEMENTATION_SUMMARY.md
- **Seguridad:** SECURITY_SUMMARY.md
