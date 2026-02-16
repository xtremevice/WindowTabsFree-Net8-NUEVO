# 🎯 Comandos Rápidos - WindowTabsFree

Tarjeta de referencia rápida con los comandos más usados.

---

## 📥 Descargar por Primera Vez

```bash
git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git
cd WindowTabsFree-Net8-NUEVO
```

---

## 🚀 Ejecutar

### Opción 1: Ejecución Directa (Más Rápido)
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Opción 2: Ejecutable Compilado

**Linux:**
```bash
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI
```

**Windows:**
```powershell
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained -o .\publish\win-x64
.\publish\win-x64\WindowTabsFree.UI.exe
```

**macOS:**
```bash
./publish-macos-arm64.sh
./publish/macos-arm64/WindowTabsFree.UI
```

---

## 🔄 Actualizar a la Última Versión

```bash
cd WindowTabsFree-Net8-NUEVO
git pull origin main
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

## 🛠️ Comandos de Desarrollo

```bash
# Compilar
dotnet build

# Compilar y ejecutar
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Limpiar compilación
dotnet clean

# Ver versión de .NET
dotnet --version
```

---

## 📋 Verificar Requisitos

```bash
# .NET 8 instalado?
dotnet --version
# Debe mostrar 8.0.x

# X11 disponible? (Linux)
echo $DISPLAY
# Debe mostrar :0 o :1

# libX11 instalada? (Linux)
ldconfig -p | grep libX11
```

---

## 🆘 Instalar Dependencias

**Linux (Ubuntu/Debian):**
```bash
# .NET 8 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0

# libX11
sudo apt-get install libx11-6
```

**Linux (Fedora/RHEL):**
```bash
# .NET 8 SDK
sudo dnf install dotnet-sdk-8.0

# libX11
sudo dnf install libX11
```

**macOS:**
```bash
# .NET 8 SDK
brew install --cask dotnet-sdk

# O descarga desde: https://dotnet.microsoft.com/download/dotnet/8.0
```

**Windows:**
- Descarga .NET 8 SDK desde: https://dotnet.microsoft.com/download/dotnet/8.0

---

## 📖 Documentación

| Archivo | Descripción |
|---------|-------------|
| **COMO_DESCARGAR_Y_EJECUTAR.md** | Guía completa de descarga ⭐ |
| LEEME_PRIMERO.md | Introducción al proyecto |
| QUICKSTART_ES.md | Inicio rápido en español |
| README.md | Documentación completa |
| GUIA_USO_MEJORADA.md | Características avanzadas |

---

## 💡 Atajos Útiles

### Crear Alias (Linux/macOS)

Agregar a `~/.bashrc` o `~/.zshrc`:
```bash
alias wt="cd ~/WindowTabsFree-Net8-NUEVO && dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj"
```

Luego:
```bash
source ~/.bashrc
wt  # Ejecuta WindowTabsFree desde cualquier lugar
```

### Crear Función (Windows PowerShell)

Agregar a tu `$PROFILE`:
```powershell
function wt { 
    cd C:\WindowTabsFree-Net8-NUEVO
    dotnet run --project src\WindowTabsFree.UI\WindowTabsFree.UI.csproj 
}
```

Luego:
```powershell
. $PROFILE
wt  # Ejecuta WindowTabsFree desde cualquier lugar
```

---

## 🎯 Resumen de Un Vistazo

| Acción | Comando |
|--------|---------|
| Clonar | `git clone https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO.git` |
| Entrar | `cd WindowTabsFree-Net8-NUEVO` |
| Ejecutar | `dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj` |
| Actualizar | `git pull origin main` |
| Compilar | `dotnet build` |
| Compilar Linux | `./publish-linux.sh` |
| Compilar macOS | `./publish-macos-arm64.sh` |
| Ver versión .NET | `dotnet --version` |

---

## 🔗 Enlaces Rápidos

- **Repositorio:** https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO
- **Descargar .NET 8:** https://dotnet.microsoft.com/download/dotnet/8.0
- **Releases (próximamente):** https://github.com/xtremevice/WindowTabsFree-Net8-NUEVO/releases

---

**¡Guarda esta tarjeta para consulta rápida! 📌**
