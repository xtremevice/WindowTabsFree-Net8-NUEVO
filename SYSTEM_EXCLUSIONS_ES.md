# Exclusiones de Aplicaciones del Sistema

Este documento describe las aplicaciones del sistema predeterminadas que se excluyen de la lista de ventanas.

## Resumen

Por defecto, WindowTabsFree excluye aplicaciones comunes del sistema y componentes de la interfaz para mantener la lista de ventanas limpia y enfocada en las aplicaciones del usuario. Estas exclusiones se configuran en la lista `ExcludedApplications` en la configuración.

## Aplicaciones Excluidas por Defecto

### Procesos del Sistema Windows

| Nombre del Proceso | Descripción |
|-------------------|-------------|
| `explorer` | Explorador de Windows |
| `taskmgr` | Administrador de Tareas |
| `dwm` | Administrador de Ventanas del Escritorio |
| `SearchUI` | Interfaz de Búsqueda de Windows |
| `SearchApp` | Aplicación de Búsqueda de Windows |
| `ShellExperienceHost` | Host de Experiencia del Shell de Windows |
| `ApplicationFrameHost` | Host del marco de aplicaciones UWP |
| `TextInputHost` | Host de Entrada de Texto de Windows |
| `LockApp` | Pantalla de Bloqueo de Windows |
| `StartMenuExperienceHost` | Menú Inicio de Windows |
| `SystemSettings` | Configuración del Sistema de Windows |

### Procesos del Sistema macOS

| Nombre del Proceso | Descripción |
|-------------------|-------------|
| `Dock` | Dock de macOS |
| `Control Center` / `ControlCenter` | Centro de Control de macOS |
| `NotificationCenter` / `Notification Center` | Centro de Notificaciones de macOS |
| `SystemUIServer` | Servidor de UI del Sistema de macOS |
| `WindowServer` | Servidor de Ventanas de macOS |
| `loginwindow` | Ventana de Inicio de Sesión de macOS |
| `CoreServicesUIAgent` | Agente de UI de Servicios Principales de macOS |
| `UserEventAgent` | Agente de Eventos de Usuario de macOS |
| `Problem Reporter` / `ProblemReporter` | Reportador de Problemas de macOS |
| `Spotlight` | Búsqueda Spotlight de macOS |

### Procesos del Sistema Linux

| Nombre del Proceso | Descripción |
|-------------------|-------------|
| `gnome-shell` | Shell del Escritorio GNOME |
| `plasmashell` | Shell del Escritorio KDE Plasma |
| `xfce4-panel` | Panel del Escritorio XFCE |
| `lxpanel` | Panel del Escritorio LXDE |
| `mate-panel` | Panel del Escritorio MATE |
| `cinnamon` | Entorno de Escritorio Cinnamon |

## Personalizar Exclusiones

Puedes personalizar la lista de aplicaciones excluidas editando el archivo `settings.json` ubicado en:

- **Windows**: `%APPDATA%\WindowTabsFree\settings.json`
- **macOS**: `~/Library/Application Support/WindowTabsFree/settings.json`
- **Linux**: `~/.config/WindowTabsFree/settings.json`

### Ejemplo de Configuración

```json
{
  "ExcludedApplications": [
    "explorer",
    "taskmgr",
    "Dock",
    "Control Center",
    "gnome-shell",
    "MiAppPersonalizada"
  ]
}
```

### Agregar Exclusiones Personalizadas

Para excluir aplicaciones adicionales:

1. Encuentra el nombre del proceso de la aplicación que deseas excluir
   - **Windows**: Usa el Administrador de Tareas (pestaña Detalles)
   - **macOS**: Usa el Monitor de Actividad
   - **Linux**: Usa `ps aux` o el Monitor del Sistema
2. Agrega el nombre del proceso (sin extensión .exe) al array `ExcludedApplications`
3. Guarda el archivo
4. Reinicia WindowTabsFree para que los cambios surtan efecto

### Eliminar Exclusiones

Para hacer visible una aplicación del sistema en la lista de ventanas:

1. Abre `settings.json`
2. Elimina el nombre del proceso del array `ExcludedApplications`
3. Guarda el archivo
4. Reinicia WindowTabsFree

## ¿Por Qué Excluir Aplicaciones del Sistema?

Las aplicaciones del sistema se excluyen típicamente porque:

1. **Interfaz Limpia**: Los procesos del sistema no necesitan gestión de ventanas
2. **Estabilidad**: Manipular ventanas del sistema puede causar comportamiento inesperado
3. **Enfoque del Usuario**: La mayoría de los usuarios quieren gestionar solo sus propias ventanas de aplicaciones
4. **Rendimiento**: Menos ventanas para enumerar y mostrar

## Notas

- La coincidencia de nombres de procesos es **insensible a mayúsculas/minúsculas**
- Las exclusiones se aplican a todas las operaciones de enumeración de ventanas
- Se incluyen tanto coincidencias exactas como variantes con/sin espacios para compatibilidad
- La lista de exclusiones se carga cuando la aplicación inicia

## Solución de Problemas

### La Aplicación del Sistema Sigue Apareciendo

Si una aplicación del sistema todavía aparece en la lista de ventanas:

1. Verifica el nombre exacto del proceso usando el monitor de procesos de tu sistema
2. Revisa problemas de sensibilidad a mayúsculas (aunque la coincidencia es insensible a mayúsculas)
3. Asegúrate de que el nombre del proceso coincida exactamente (algunos procesos pueden tener nombres diferentes en versiones distintas del SO)
4. Reinicia WindowTabsFree después de hacer cambios

### Una Aplicación de Usuario Está Excluida

Si una aplicación de usuario está siendo excluida:

1. Verifica si su nombre de proceso coincide con alguno en la lista de exclusiones
2. Elimínala del array `ExcludedApplications` si es necesario
3. Ten cuidado con nombres comunes (ej. "explorer" podría ser un gestor de archivos o un navegador web)
