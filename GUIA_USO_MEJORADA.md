# Guía de Uso - Detección de Ventanas y Agrupación Mejorada

## 🎉 Nuevas Funcionalidades Implementadas

### 1. Detección Mejorada de Ventanas

**Antes:**
- No detectaba todas las ventanas abiertas
- Filtraba ventanas sin título

**Ahora:**
- ✅ Detecta TODAS las ventanas visibles
- ✅ Incluye ventanas aunque no tengan título (si tienen nombre de proceso)
- ✅ Solo excluye ventanas del sistema sin ProcessName

### 2. Auto-Agrupación por Aplicación (Individual)

**Antes:**
- Botón "Auto-Group by App" agrupaba TODAS las aplicaciones
- No había control individual

**Ahora:**
- ✅ Cada aplicación tiene su propio checkbox "Auto-group"
- ✅ Solo se agrupan las apps que tienen el checkbox activado
- ✅ La configuración se guarda y persiste

**Cómo Usar:**
1. Encuentra la aplicación en la lista de ventanas
2. Marca el checkbox "Auto-group" junto al nombre de la app
3. Si la app tiene 2+ ventanas, se crea un grupo automáticamente
4. Nuevas ventanas de esa app se agregan al grupo automáticamente
5. Desmarca el checkbox para desactivar y eliminar el grupo

### 3. Información Completa de Ventanas

Cada ventana ahora muestra:

```
┌─────────────────────────────────────────────────┐
│ 📱 chrome (3 windows)          ☑ Auto-group     │
│                                                  │
│ Google - Mozilla Firefox                        │
│                                                  │
│ Size: 1920 × 1080    State: Normal             │
│                                                  │
│ [Focus] [Minimize] [Maximize] [Restore] [Close] │
│ [➕ Add to Group]                                │
└─────────────────────────────────────────────────┘
```

**Información mostrada:**
- 📱 **Nombre de la aplicación** (ProcessName) en grande
- **Cantidad de ventanas** de esa aplicación
- **Checkbox Auto-group** para activar agrupación automática
- **Título de la ventana**
- **Tamaño** (ancho × alto)
- **Estado** (Normal, Minimized, Maximized)
- **Botones de acción** (Focus, Minimize, Maximize, Restore, Close)
- **Botón "➕ Add to Group"** (verde, destacado)

### 4. Agrupación Manual Mejorada

**Cómo agregar una ventana a un grupo:**
1. Haz clic en el grupo deseado (panel izquierdo)
2. El botón "➕ Add to Group" se activará
3. Encuentra la ventana que quieres agregar
4. Haz clic en "➕ Add to Group" de esa ventana
5. La ventana se agrega al grupo seleccionado

## 📊 Ejemplos de Uso

### Ejemplo 1: Auto-Agrupar Chrome

Tienes 5 ventanas de Chrome abiertas:

1. Encuentra cualquier ventana de Chrome en la lista
2. Verás: `📱 chrome (5 windows) ☐ Auto-group`
3. Marca el checkbox "Auto-group"
4. Se crea automáticamente un grupo "chrome Windows"
5. Las 5 ventanas aparecen en el grupo
6. Abre una nueva ventana de Chrome → se agrega automáticamente

### Ejemplo 2: Agrupar Manualmente Proyectos

Quieres agrupar ventanas de diferentes apps para un proyecto:

1. Clic en "New Tab Group"
2. Nombra el grupo: "Proyecto X"
3. Haz clic en el grupo "Proyecto X" para seleccionarlo
4. Para cada ventana que quieras agregar:
   - Encuentra la ventana en la lista
   - Clic en "➕ Add to Group"
5. El grupo ahora contiene todas tus ventanas del proyecto

### Ejemplo 3: Workflow de Desarrollo

Configuración típica para desarrollo:

```
Auto-group activado para:
☑ code (VS Code) - 3 windows
☑ chrome - 4 windows
☑ gnome-terminal - 2 windows

Grupos manuales:
📁 Backend
   - VS Code (backend project)
   - Terminal (server running)
   
📁 Frontend
   - VS Code (frontend project)
   - Chrome (localhost:3000)
   - Terminal (npm run dev)
```

## 🔧 Configuración

### Archivo de Configuración

Las preferencias de auto-agrupación se guardan en:
- Linux: `~/.config/WindowTabsFree/settings.json`
- Windows: `%APPDATA%\WindowTabsFree\settings.json`
- macOS: `~/Library/Application Support/WindowTabsFree/settings.json`

### Estructura de Configuración

```json
{
  "ApplicationGroupSettings": [
    {
      "ProcessName": "chrome",
      "IsAutoGroupEnabled": true,
      "LastModifiedAt": "2026-02-16T10:00:00Z"
    },
    {
      "ProcessName": "code",
      "IsAutoGroupEnabled": true,
      "LastModifiedAt": "2026-02-16T10:05:00Z"
    }
  ],
  "TabGroups": [
    {
      "Id": "abc123",
      "Name": "chrome Windows",
      "ApplicationName": "chrome",
      "IsAutoGrouped": true,
      "WindowHandles": [...],
      "ActiveWindowIndex": 0,
      "Color": "#4ECDC4"
    }
  ]
}
```

## ❓ Preguntas Frecuentes

### ¿Por qué algunas ventanas no aparecen?

**R:** La aplicación ahora muestra todas las ventanas visibles. Si una ventana no aparece:
1. Verifica que la ventana esté realmente visible (no minimizada a la bandeja)
2. Algunas ventanas del sistema se excluyen por seguridad
3. Haz clic en "Refresh Now" para actualizar la lista

### ¿Cómo desactivo el auto-agrupamiento?

**R:** Hay dos formas:
1. **Por aplicación:** Desmarca el checkbox "Auto-group" de esa app
2. **Todos:** Haz clic en el botón "Remove Auto-Groups" (naranja)

### ¿Los grupos se guardan al cerrar la aplicación?

**R:** Sí, tanto los grupos manuales como las preferencias de auto-agrupación se guardan automáticamente en `settings.json`.

### ¿Puedo tener grupos automáticos Y manuales?

**R:** ¡Sí! Puedes:
- Tener auto-grupos para apps específicas (chrome, code, etc.)
- Crear grupos manuales para proyectos o workflows
- Mezclar ventanas de diferentes apps en grupos manuales

### ¿Qué pasa si cierro una ventana que está en un grupo?

**R:** 
- La ventana se elimina del grupo automáticamente
- Si era la última ventana del grupo manual, el grupo permanece vacío
- Los grupos auto-generados se actualizan automáticamente cada 2 segundos

## 🎨 Interfaz Visual

### Panel Izquierdo - Grupos

```
Tab Groups
┌─────────────────────────────────┐
│ chrome Windows                  │
│ App: chrome                     │
│ Windows: 5                      │
│ [◄ Prev Tab] [Next Tab ►]      │
│ Active Tab: Tab 2 of 5          │
└─────────────────────────────────┘
```

### Panel Derecho - Ventanas

```
Active Windows (12)
┌─────────────────────────────────┐
│ 📱 chrome (5 windows) ☑ Auto... │
│ Google - Search                 │
│ Size: 1920 × 1080  State: Normal│
│ [Focus] [Min] [Max] [Rest] [×]  │
│ [➕ Add to Group]                │
├─────────────────────────────────┤
│ 📱 code (3 windows) ☑ Auto...   │
│ main.py - Visual Studio Code   │
│ Size: 1600 × 900  State: Max... │
│ [Focus] [Min] [Max] [Rest] [×]  │
│ [➕ Add to Group]                │
└─────────────────────────────────┘
```

## 🚀 Tips y Trucos

### Tip 1: Organización Rápida
Activa auto-group para tus apps principales al inicio del día, luego crea grupos manuales para proyectos específicos.

### Tip 2: Navegación con Teclado
Usa los botones "◄ Prev Tab" y "Next Tab ►" para cambiar rápidamente entre ventanas de la misma app.

### Tip 3: Colores Distintivos
Los grupos auto-generados tienen colores automáticos basados en el nombre de la app para fácil identificación.

### Tip 4: Refresh Automático
La lista se actualiza cada 2 segundos, pero puedes forzar un refresh con el botón "Refresh Now".

## 📝 Notas Técnicas

- **Auto-refresh:** La lista de ventanas se actualiza cada 2 segundos
- **Persistencia:** Configuraciones se guardan inmediatamente en JSON
- **Compatibilidad:** Funciona en Windows, Linux (X11) y macOS
- **Performance:** Optimizado para manejar cientos de ventanas sin lag

## 🐛 Solución de Problemas

### Problema: Checkbox auto-group no aparece
**Solución:** Asegúrate de que tienes 2+ ventanas de esa aplicación abiertas.

### Problema: Botón "Add to Group" deshabilitado
**Solución:** Primero selecciona un grupo en el panel izquierdo.

### Problema: Grupo automático no se crea
**Solución:** 
1. Verifica que el checkbox esté marcado
2. Asegúrate de tener al menos 2 ventanas de esa app
3. Haz clic en "Refresh Now"

### Problema: Ventanas duplicadas en la lista
**Solución:** Esto es normal si una app tiene múltiples ventanas. Cada entrada es una ventana diferente.

## 📞 Soporte

Si encuentras algún problema o tienes sugerencias:
1. Revisa esta guía primero
2. Verifica los logs de la aplicación
3. Reporta issues en GitHub con detalles específicos
