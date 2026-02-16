# Auto-Agrupación y Visualización de Tabs - Guía Completa

## 🎉 Nuevas Funcionalidades Implementadas

Este documento explica las mejoras implementadas para auto-agrupación y visualización de tabs.

## 📋 Problemas Resueltos

### 1. ✅ Auto-Grouping Ahora Funciona al Hacer Click

**Problema Anterior:**
- El botón "Auto-Group by App" no hacía nada
- Primero había que marcar el checkbox "Auto-group" de cada aplicación
- Proceso confuso y poco intuitivo

**Solución Implementada:**
- Click en "Auto-Group by App" **automáticamente agrupa** todas las apps con 2+ ventanas
- **NO requiere** marcar checkboxes previamente
- Automáticamente marca los checkboxes después de agrupar
- Proceso simple e intuitivo

### 2. ✅ Visualización de Tabs en Grupos

**Problema Anterior:**
- Los grupos solo mostraban el nombre y cantidad de ventanas
- No se podían ver las ventanas individuales
- No había forma de saber qué ventanas estaban en el grupo

**Solución Implementada:**
- Cada grupo muestra una lista de **tabs** (pestañas)
- Cada tab muestra:
  - **Título** de la ventana
  - **Nombre** de la aplicación
  - **Estado** (activa o inactiva)
  - **Botón** para enfocar esa ventana específica

### 3. ✅ Ventana Activa Sobresale Visualmente

**Problema Anterior:**
- No había forma de saber cuál ventana del grupo estaba activa
- Todas las ventanas se veían iguales

**Solución Implementada:**
- **Tab activa:** Fondo verde (#C8E6C9) con borde verde grueso
- **Tabs inactivas:** Fondo blanco con borde azul
- **Indicador visual:** Checkmark (✓) para activa, icono 📄 para inactivas
- **Etiqueta:** "[ACTIVE TAB]" en la ventana activa

## 🎨 Interfaz Visual

### Panel de Grupos (Izquierda)

```
┌─────────────────────────────────────────┐
│ Tab Groups                              │
├─────────────────────────────────────────┤
│                                         │
│ ┌─────────────────────────────────────┐│
│ │ Chrome Windows                   [×]││
│ │ App: chrome                         ││
│ │ Windows: 3                          ││
│ │                                     ││
│ │ [◄ Prev Tab]  [Next Tab ►]         ││
│ │                                     ││
│ │ Active Tab:                        ││
│ │ Tab 1 of 3                         ││
│ │                                     ││
│ │ Tabs:                              ││
│ │ ┌───────────────────────────────┐  ││
│ │ │ ✓ Google Search               │  ││ Verde
│ │ │ App: chrome                   │  ││ (ACTIVA)
│ │ │ [ACTIVE TAB]                  │  ││
│ │ │ [Focus This Tab]              │  ││
│ │ └───────────────────────────────┘  ││
│ │ ┌───────────────────────────────┐  ││
│ │ │ 📄 Gmail - Inbox              │  ││ Blanco
│ │ │ App: chrome                   │  ││ (Inactiva)
│ │ │ [Focus This Tab]              │  ││
│ │ └───────────────────────────────┘  ││
│ │ ┌───────────────────────────────┐  ││
│ │ │ 📄 YouTube                    │  ││ Blanco
│ │ │ App: chrome                   │  ││ (Inactiva)
│ │ │ [Focus This Tab]              │  ││
│ │ └───────────────────────────────┘  ││
│ └─────────────────────────────────────┘│
└─────────────────────────────────────────┘
```

### Colores y Estilos

**Tab Activa:**
- Fondo: Verde claro (#C8E6C9)
- Borde: Verde oscuro (#4CAF50), grosor 3px
- Icono: ✓ (checkmark) verde
- Texto: En negrita
- Etiqueta: "[ACTIVE TAB]" en verde

**Tab Inactiva:**
- Fondo: Blanco
- Borde: Azul (#2196F3), grosor 2px
- Icono: 📄 (documento)
- Texto: Normal
- Sin etiqueta especial

## 📖 Cómo Usar

### Paso 1: Abrir Múltiples Ventanas

Abre varias ventanas de la misma aplicación. Por ejemplo:
- 3 ventanas de Chrome
- 2 ventanas de Terminal
- 4 ventanas de VS Code

### Paso 2: Auto-Agrupar

1. Inicia WindowTabsFree
2. Click en el botón verde **"Auto-Group by App"** en la barra superior
3. La aplicación automáticamente:
   - Crea un grupo por cada app con 2+ ventanas
   - Marca el checkbox "Auto-group" de cada app agrupada
   - Muestra las ventanas en forma de tabs

### Paso 3: Ver las Tabs

En el panel izquierdo (Tab Groups), cada grupo ahora muestra:
- Nombre del grupo (ej: "Chrome Windows")
- Cantidad de ventanas
- **Lista de tabs** con título de cada ventana
- **Tab activa** resaltada en verde

### Paso 4: Navegar Entre Tabs

Tienes **3 formas** de cambiar entre ventanas:

**Opción 1: Botones de Navegación**
- Click en **"◄ Prev Tab"** para ir a la ventana anterior
- Click en **"Next Tab ►"** para ir a la siguiente ventana
- Navegación circular (después de la última vuelve a la primera)

**Opción 2: Botón Individual**
- Click en **"Focus This Tab"** en cualquier tab
- Enfoca directamente esa ventana específica

**Opción 3: Desde Panel Derecho**
- Click en checkbox "Auto-group" de una aplicación
- Usa botones Focus, Minimize, etc.

### Paso 5: Visual de Tab Activa

Cuando cambias de tab:
1. La tab anterior se vuelve blanca (inactiva)
2. La nueva tab se vuelve verde (activa)
3. El indicador "Active Tab: Tab X of Y" se actualiza
4. La ventana recibe el focus del sistema operativo

## 🔧 Funcionalidades Técnicas

### Auto-Grouping Inteligente

**Criterios de Agrupación:**
- Solo agrupa aplicaciones con **2 o más ventanas**
- Ignora ventanas de sistema
- Respeta lista de exclusión de apps
- Automático al hacer click en el botón

**Persistencia:**
- Los grupos se guardan en `~/.config/WindowTabsFree/settings.json` (Linux)
- O en `%APPDATA%\WindowTabsFree\settings.json` (Windows)
- El estado "Auto-group" se persiste por aplicación
- Los grupos se recrean al reiniciar la app

### Sincronización en Tiempo Real

- La lista de ventanas se actualiza cada **2 segundos**
- Los grupos se actualizan después de cada operación
- La tab activa se sincroniza con el focus real del SO
- Ventanas cerradas se eliminan automáticamente de grupos

## 🎯 Ejemplos de Uso

### Ejemplo 1: Desarrollo Web

**Escenario:** Desarrollando una app web

**Ventanas Abiertas:**
- VS Code: proyecto backend
- VS Code: proyecto frontend
- Chrome: localhost:3000
- Chrome: localhost:8080  
- Chrome: documentación
- Terminal: servidor backend
- Terminal: servidor frontend

**Después de Auto-Group:**

```
Grupo 1: VS Code Windows (2 ventanas)
  ✓ backend/src/app.js [ACTIVE]
  📄 frontend/src/App.tsx

Grupo 2: chrome Windows (3 ventanas)
  ✓ localhost:3000 - React App [ACTIVE]
  📄 localhost:8080 - API Server
  📄 MDN Web Docs

Grupo 3: gnome-terminal Windows (2 ventanas)
  ✓ npm run dev [ACTIVE]
  📄 node server.js
```

### Ejemplo 2: Multitarea General

**Escenario:** Trabajo general multitarea

**Ventanas Abiertas:**
- Brave: Email
- Brave: Calendar
- Brave: YouTube
- Terminal: system monitor
- Terminal: file operations

**Después de Auto-Group:**

```
Grupo 1: Brave Browser Windows (3 ventanas)
  ✓ Gmail - Inbox [ACTIVE]
  📄 Google Calendar
  📄 YouTube - Home

Grupo 2: Terminal Windows (2 ventanas)
  ✓ htop [ACTIVE]
  📄 ~/Documents
```

## 🐛 Solución de Problemas

### El botón "Auto-Group by App" no crea grupos

**Posibles causas:**
1. No hay aplicaciones con 2+ ventanas abiertas
2. Las aplicaciones están en la lista de exclusión

**Solución:**
1. Verifica que tienes al menos 2 ventanas de la misma app
2. Revisa settings.json para lista de exclusión

### Las tabs no se actualizan

**Posibles causas:**
1. El timer de auto-refresh está detenido
2. Error en la sincronización

**Solución:**
1. Click en "Refresh Now" manualmente
2. Reinicia la aplicación

### La tab activa no se resalta

**Posibles causas:**
1. El índice activo no está sincronizado
2. WindowsInfo no se está poblando

**Solución:**
1. Click en "Next Tab" para forzar actualización
2. Verifica que la ventana existe en el sistema

### Los títulos de tabs están vacíos

**Posibles causas:**
1. Ventanas sin título (Terminal, juegos, etc.)
2. DisplayTitle no funciona correctamente

**Solución:**
- Normal para algunas apps
- Se muestra `[ProcessName]` en lugar de título vacío
- Ejemplo: `[Terminal]` para ventanas de terminal sin título

## 🔮 Próximas Mejoras

Funcionalidades planeadas:

1. **Drag & Drop:** Arrastrar ventanas entre grupos
2. **Renombrar tabs:** Click derecho para renombrar
3. **Cerrar tab:** Botón × en cada tab
4. **Reorganizar tabs:** Arrastrar para reordenar
5. **Hotkeys globales:** Atajos de teclado del sistema
6. **Temas:** Personalizar colores de tabs
7. **Iconos reales:** Mostrar iconos de apps en lugar de emojis
8. **Minimizar grupo:** Minimizar todas las ventanas del grupo
9. **Historial:** Ver ventanas cerradas recientemente
10. **Búsqueda:** Buscar ventanas por título o app

## 📚 Archivos Técnicos Modificados

Para desarrolladores interesados en la implementación:

1. **WindowManagerService.cs**
   - Método `AutoGroupByApplication()` modificado
   - Elimina filtro de `IsAutoGroupEnabled`
   - Activa auto-group automáticamente

2. **TabGroup.cs**
   - Nueva propiedad `WindowsInfo`
   - Marcada con `[JsonIgnore]` (no se persiste)

3. **WindowInfo.cs**
   - Nueva propiedad `IsActiveInGroup`
   - Para highlighting visual

4. **MainWindowViewModel.cs**
   - Método `RefreshTabGroups()` mejorado
   - Popula `WindowsInfo` con datos reales
   - Marca `IsActiveInGroup` basado en índice

5. **MainWindow.axaml**
   - Nuevo panel de tabs
   - Dos templates: activa (verde) e inactiva (blanco)
   - Binding a `IsActiveInGroup` para mostrar/ocultar

## 📞 Soporte

Si encuentras problemas o tienes preguntas:

1. Revisa esta guía completa
2. Verifica la sección de Solución de Problemas
3. Consulta los logs de la aplicación
4. Reporta issues en el repositorio de GitHub

---

**Versión:** 1.0  
**Última actualización:** 2026-02-16  
**Plataformas:** Windows, Linux, macOS
