# Cómo Usar Este Proyecto - WindowTabsFree-Net8

## 📁 ¿Qué es esto?

**WindowTabsFree-Net8** es el proyecto completo y organizado de WindowTabsFree para .NET 8. Este directorio contiene TODO lo necesario para trabajar en el proyecto de forma independiente.

## ✨ ¿Qué contiene?

### 1. Código Fuente Completo
```
src/
├── WindowTabsFree.Common/      # Modelos e interfaces compartidas
├── WindowTabsFree.Core/        # Lógica de negocio
├── WindowTabsFree.Services/    # Implementaciones por plataforma
├── WindowTabsFree.UI/          # Interfaz de usuario Avalonia
└── WindowTabsFree.TestConsole/ # Utilidad de pruebas
```

### 2. Documentación (14 archivos)
- **README.md** - Documentación principal (este directorio)
- **README_NET8.md** - Documentación detallada completa
- Guías rápidas (inglés y español)
- Guías de características
- Guías específicas de plataforma
- Documentación técnica

### 3. Scripts de Compilación
- `publish-linux.sh` - Compilar para Linux
- `publish-macos-arm64.sh` - Compilar para macOS Apple Silicon
- `check-macos-requirements.sh` - Verificar requisitos en macOS

## 🚀 Inicio Rápido

### Opción 1: Ejecutar Directamente
```bash
cd WindowTabsFree-Net8
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Opción 2: Compilar Primero
```bash
cd WindowTabsFree-Net8
dotnet build WindowTabsFreeNet.sln
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### Opción 3: Crear Ejecutable Standalone

**Linux:**
```bash
cd WindowTabsFree-Net8
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI
```

**macOS (Apple Silicon):**
```bash
cd WindowTabsFree-Net8
./publish-macos-arm64.sh
./publish/macos-arm64/WindowTabsFree.UI
```

**Windows:**
```bash
cd WindowTabsFree-Net8
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained -o publish/win-x64
publish\win-x64\WindowTabsFree.UI.exe
```

## 📖 Documentación Recomendada

### Para Empezar
1. **[README.md](README.md)** - Lee esto primero
2. **[QUICKSTART_ES.md](QUICKSTART_ES.md)** - Guía rápida en español
3. **[README_NET8.md](README_NET8.md)** - Documentación completa

### Para Usar Características
- **[GUIA_AGRUPACION.md](GUIA_AGRUPACION.md)** - Auto-agrupación y navegación de tabs
- **[GUIA_USO_MEJORADA.md](GUIA_USO_MEJORADA.md)** - Guía de características mejoradas
- **[AUTO_AGRUPACION_Y_TABS.md](AUTO_AGRUPACION_Y_TABS.md)** - Sistema de tabs

### Para macOS
- **[GUIA_MAC_APPLE_SILICON.md](GUIA_MAC_APPLE_SILICON.md)** - Guía completa para M1/M2/M3
- **[MACOS_GUIDE.md](MACOS_GUIDE.md)** - Guía general de macOS

### Técnico
- **[LIMITACION_TABS_OVERLAY.md](LIMITACION_TABS_OVERLAY.md)** - Explicación de limitaciones
- **[RESUMEN_CAMBIOS_AGRUPACION.md](RESUMEN_CAMBIOS_AGRUPACION.md)** - Resumen de cambios

## 🎯 Características Principales

### Gestión de Ventanas
- ✅ Enumerar ventanas abiertas (Windows/Linux/macOS)
- ✅ Controlar ventanas: Focus, Minimize, Maximize, Restore, Close
- ✅ Actualización en tiempo real cada 2 segundos

### Agrupación de Tabs
- ✅ Auto-agrupación por aplicación (checkbox individual)
- ✅ Grupos manuales para proyectos
- ✅ Navegación entre tabs (Prev/Next)
- ✅ Resaltado visual de tab activa (verde)
- ✅ Configuración persistente en JSON

### Ventana Flotante
- ✅ Ventana compacta (450x600) con toda la información
- ✅ Always-on-top opcional
- ✅ Auto-minimiza ventana principal
- ✅ Visualización completa de todos los tabs
- ✅ Botones Focus para cada tab

## 🛠️ Desarrollo

### Requisitos
- .NET 8 SDK
- **Linux**: X11 display server
- **macOS**: Accessibility permissions

### Comandos Útiles

**Compilar:**
```bash
dotnet build WindowTabsFreeNet.sln
```

**Limpiar:**
```bash
dotnet clean WindowTabsFreeNet.sln
```

**Restaurar paquetes:**
```bash
dotnet restore WindowTabsFreeNet.sln
```

**Ejecutar tests (consola):**
```bash
dotnet run --project src/WindowTabsFree.TestConsole/WindowTabsFree.TestConsole.csproj
```

### Estructura de Proyectos

1. **WindowTabsFree.Common** - Modelos compartidos
   - `WindowInfo`, `TabGroup`, `AppSettings`
   - Interfaces: `IWindowService`, `IConfigurationService`

2. **WindowTabsFree.Core** - Lógica de negocio
   - `WindowManagerService` - Gestión de ventanas y grupos
   - `ConfigurationService` - Persistencia en JSON

3. **WindowTabsFree.Services** - Implementaciones por plataforma
   - `WindowsWindowService` - Win32 P/Invoke
   - `LinuxWindowService` - X11 P/Invoke
   - `MacOSWindowService` - Core Graphics + AppleScript
   - `WindowServiceFactory` - Detecta plataforma

4. **WindowTabsFree.UI** - Interfaz Avalonia
   - `MainWindow` - Ventana principal
   - `FloatingWindowManager` - Ventana flotante
   - `MainWindowViewModel` - ViewModel principal

5. **WindowTabsFree.TestConsole** - Utilidad de pruebas
   - Prueba enumeración de ventanas
   - Prueba operaciones de ventanas

## 📍 Configuración

Los settings se guardan en:
- **Linux**: `~/.config/WindowTabsFree/settings.json`
- **macOS**: `~/Library/Application Support/WindowTabsFree/settings.json`
- **Windows**: `%APPDATA%\WindowTabsFree\settings.json`

### Ejemplo de settings.json
```json
{
  "TabGroups": [
    {
      "Id": "abc123",
      "Name": "Chrome Windows",
      "WindowHandles": [12345, 12346, 12347],
      "ActiveWindowIndex": 0,
      "IsAutoGrouped": true,
      "ApplicationName": "chrome"
    }
  ],
  "ApplicationGroupSettings": [
    {
      "ProcessName": "chrome",
      "IsAutoGroupEnabled": true,
      "LastModifiedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "TopmostEnabled": false
}
```

## 🎨 Workflow Recomendado

### 1. Configuración Inicial
```bash
# Navegar al proyecto
cd WindowTabsFree-Net8

# Compilar
dotnet build

# Ejecutar
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

### 2. Agrupar Aplicaciones
1. Abrir múltiples ventanas de una aplicación (ej: 3 ventanas de Chrome)
2. En la lista de ventanas, marcar checkbox "Auto-group" en Chrome
3. Se crea automáticamente un grupo con las 3 ventanas

### 3. Usar Ventana Flotante
1. Click en botón "🪟 Floating Window"
2. La ventana principal se minimiza automáticamente
3. Ventana flotante muestra todos los grupos y tabs
4. Activar "📌 Pin On Top" para mantenerla siempre visible

### 4. Navegar Entre Tabs
- Click "◄ Prev Tab" o "Next Tab ►"
- O click "Focus This Tab" en cualquier tab individual
- La tab activa se resalta en verde

## 🐛 Solución de Problemas

### No compila
```bash
# Verificar .NET 8
dotnet --version
# Debe mostrar 8.0.x

# Limpiar y restaurar
dotnet clean
dotnet restore
dotnet build
```

### Linux: No detecta ventanas
```bash
# Verificar X11
echo $DISPLAY
# Debe mostrar :0 o similar

# Verificar libX11
ldconfig -p | grep libX11
```

### macOS: No puede controlar ventanas
1. System Preferences → Security & Privacy → Privacy → Accessibility
2. Agregar WindowTabsFree.UI a la lista
3. Activar el checkbox

## 📦 Desplegar

### Copiar a Otro Lugar
```bash
# Este directorio es autocontenido, simplemente cópialo
cp -r WindowTabsFree-Net8 /ruta/destino/
cd /ruta/destino/WindowTabsFree-Net8
dotnet build
```

### Compartir con Otros
```bash
# Crear un tarball
tar czf WindowTabsFree-Net8.tar.gz WindowTabsFree-Net8/

# O crear un zip
zip -r WindowTabsFree-Net8.zip WindowTabsFree-Net8/
```

## 🎓 Aprendiendo el Código

### Orden Recomendado de Lectura

1. **Modelos** (`src/WindowTabsFree.Common/Models/`)
   - `WindowInfo.cs` - Información de ventana
   - `TabGroup.cs` - Grupo de tabs
   - `AppSettings.cs` - Configuración

2. **Interfaces** (`src/WindowTabsFree.Common/Interfaces/`)
   - `IWindowService.cs` - Contrato de servicio de ventanas

3. **Servicios** (`src/WindowTabsFree.Services/`)
   - `WindowServiceFactory.cs` - Cómo se detecta la plataforma
   - `Windows/WindowsWindowService.cs` - Implementación Windows
   - `Linux/LinuxWindowService.cs` - Implementación Linux
   - `macOS/MacOSWindowService.cs` - Implementación macOS

4. **Lógica de Negocio** (`src/WindowTabsFree.Core/Services/`)
   - `WindowManagerService.cs` - Gestión de ventanas y grupos
   - `ConfigurationService.cs` - Persistencia

5. **UI** (`src/WindowTabsFree.UI/`)
   - `ViewModels/MainWindowViewModel.cs` - ViewModel
   - `Views/MainWindow.axaml` - Ventana principal
   - `Views/FloatingWindowManager.axaml` - Ventana flotante

## 🚀 Próximos Pasos

Este proyecto está listo para:
- ✅ Desarrollo continuo
- ✅ Agregar nuevas características
- ✅ Mejorar UI
- ✅ Optimización de rendimiento
- ✅ Agregar tests unitarios
- ✅ Deploy en producción

## 💡 Tips

- Lee primero `README.md` para overview general
- Usa `QUICKSTART_ES.md` para empezar rápido
- Consulta documentación específica según necesites
- Los scripts de publish facilitan crear ejecutables
- La ventana flotante es la interfaz principal de trabajo

## 📞 Más Información

Para más detalles, consulta los archivos de documentación incluidos. Cada uno cubre un aspecto específico del proyecto.

---

**WindowTabsFree-Net8** - Tu proyecto .NET 8 listo para usar y desarrollar.
