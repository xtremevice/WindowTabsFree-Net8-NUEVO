# Arreglo de Detección de Ventanas en macOS

## Problema Resuelto ✅

El programa no detectaba en Mac las siguientes aplicaciones:
- ❌ Terminal / iTerm2
- ❌ Brave Browser
- ❌ GitHub Desktop
- ❌ Wakfu (juego)
- ❌ Otras apps sin título de ventana

## ¿Por qué no se detectaban?

El filtro original solo incluía ventanas que tenían un **título** (Title). Pero muchas aplicaciones tienen ventanas sin título:

- **Terminal**: Las ventanas pueden no tener título
- **Brave**: Ventanas nuevas pueden estar sin título inicialmente
- **GitHub Desktop**: Algunas ventanas tienen título vacío
- **Juegos**: Raramente usan títulos de ventana estándar

## Solución Implementada

### 1. Filtro Mejorado en macOS

**Ahora se incluyen ventanas que tengan:**
- Título (Title) **O**
- Nombre de Proceso (ProcessName)

Esto significa que si una ventana no tiene título pero sabemos qué aplicación la creó, la detectamos igual.

### 2. Visualización Mejorada

**Nueva propiedad `DisplayTitle`:**
- Si la ventana tiene título → muestra el título
- Si no tiene título → muestra `[ProcessName]`
- Nunca muestra campos vacíos

### 3. Logging para Debugging

Ahora el programa muestra en consola:
```
macOS Window detected: ProcessName='Terminal', Title=''
macOS Window detected: ProcessName='Brave Browser', Title='GitHub'
macOS Window detected: ProcessName='GitHub Desktop', Title=''
macOS Window detected: ProcessName='Wakfu', Title=''
```

## Verificación del Auto-Group

✅ El checkbox "Auto-group" **ya estaba visible** en la UI:
- Aparece en cada aplicación en la lista de ventanas activas
- Permite activar auto-agrupación por aplicación individual
- Tiene tooltip explicativo

## Cómo Probar

1. **Actualizar el código:**
   ```bash
   cd WindowTabsFree
   git pull origin copilot/implement-window-service-functionality
   ```

2. **Ejecutar la aplicación:**
   ```bash
   dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
   ```

3. **Abrir las aplicaciones problemáticas:**
   - Abre Terminal
   - Abre Brave Browser
   - Abre GitHub Desktop
   - Abre Wakfu u otro juego

4. **Verificar detección:**
   - Todas las aplicaciones deben aparecer en la lista
   - Cada una debe tener el checkbox "Auto-group"
   - Puedes activar el checkbox para agruparlas automáticamente

## Ejemplo de Uso

### Antes (No funcionaba):
```
Lista de Ventanas Activas:
- Safari
- Finder
(Terminal, Brave, GitHub Desktop, Wakfu NO aparecían)
```

### Después (Funciona):
```
Lista de Ventanas Activas:
- Safari
- Finder
- 📱 Terminal (2 windows) ☑ Auto-group
  [Terminal]
- 📱 Brave Browser (3 windows) ☑ Auto-group
  GitHub - Pull Requests
- 📱 GitHub Desktop (1 windows) ☐ Auto-group
  [GitHub Desktop]
- 📱 Wakfu (1 windows) ☐ Auto-group
  [Wakfu]
```

## Archivos Modificados

1. **MacOSWindowService.cs** - Filtro mejorado de detección
2. **WindowManagerService.cs** - Filtro mejorado de ventanas manejables
3. **WindowInfo.cs** - Nueva propiedad DisplayTitle
4. **MainWindow.axaml** - Usa DisplayTitle en la UI

## Notas Técnicas

### Criterio de Detección (macOS)

```csharp
// Incluir ventana si:
windowInfo != null && 
(
    !string.IsNullOrWhiteSpace(windowInfo.Title) ||      // Tiene título O
    !string.IsNullOrWhiteSpace(windowInfo.ProcessName)   // Tiene proceso
)
```

### Criterio de Ventanas Manejables

```csharp
// Incluir en lista si:
(
    !string.IsNullOrWhiteSpace(Title) ||          // Tiene título O
    !string.IsNullOrWhiteSpace(ProcessName)       // Tiene proceso
) 
&& 
!excludedApps.Contains(ProcessName)               // Y no está excluida
```

## Preguntas Frecuentes

### ¿Por qué algunas ventanas muestran [ProcessName] en lugar del título?

Porque esas ventanas no tienen un título establecido. Esto es normal para:
- Aplicaciones de Terminal
- Algunas ventanas de navegadores
- Aplicaciones de juegos
- Ventanas de sistema

### ¿Puedo agrupar aplicaciones sin título?

✅ Sí! Ahora que se detectan correctamente, puedes:
- Activar el checkbox "Auto-group" para agruparlas automáticamente
- Agregar manualmente a grupos existentes
- Navegar entre pestañas normalmente

### ¿El auto-group funciona con estas aplicaciones?

✅ Sí! El auto-group funciona perfectamente porque se basa en el ProcessName, no en el título.

### ¿Cómo sé si mi app se está detectando?

Mira la consola de debug. Verás mensajes como:
```
macOS Window detected: ProcessName='TuApp', Title='...'
```

Si ves:
```
macOS Window filtered out: ProcessName='TuApp', Title=''
```

Significa que algo está bloqueando la detección (posiblemente en la lista de exclusión).

## Próximos Pasos

Con este arreglo, ahora puedes:

1. ✅ Ver todas tus aplicaciones en la lista
2. ✅ Activar auto-group individual por aplicación
3. ✅ Agrupar manualmente cualquier ventana
4. ✅ Navegar entre pestañas de aplicaciones sin título
5. ✅ Usar todas las funciones (Focus, Minimize, Maximize, etc.)

¡Disfruta de WindowTabsFree completamente funcional en macOS! 🎉
