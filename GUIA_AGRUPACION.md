# Guía de Uso - Agrupación Automática y Navegación de Pestañas

## 🎯 Nuevas Funcionalidades

WindowTabsFree ahora incluye **agrupación automática de ventanas** y **navegación entre pestañas** para organizar mejor tus aplicaciones abiertas.

---

## 📱 Características Principales

### 1. **Mostrar Nombres de Aplicaciones**
Cada ventana activa muestra:
- **Título de la ventana** (en negrita)
- **Nombre de la aplicación** (ProcessName en gris)
- Estado (Minimizado/Maximizado)
- Tamaño de la ventana

### 2. **Agrupación Manual**
- Haz clic en "New Tab Group" para crear un grupo personalizado
- Selecciona un grupo en el panel izquierdo
- Haz clic en "Add to Group" junto a cualquier ventana para agregarla

### 3. **Agrupación Automática** ⭐ NUEVO
- Haz clic en **"Auto-Group by App"** (botón verde)
- Automáticamente crea grupos para todas las aplicaciones con múltiples ventanas
- Por ejemplo: Si tienes 3 ventanas de Chrome y 2 de VS Code, creará:
  - Grupo "chrome Windows" con las 3 ventanas de Chrome
  - Grupo "Code Windows" con las 2 ventanas de VS Code
- Cada grupo automático tiene un color distintivo

### 4. **Navegación entre Pestañas** ⭐ NUEVO
Cada grupo muestra botones de navegación:
- **◄ Prev Tab** - Cambia a la pestaña anterior
- **Next Tab ►** - Cambia a la siguiente pestaña
- **Indicador Visual** - Muestra "Tab X of Y" (pestaña activa de total)
- La navegación es circular (después de la última vuelve a la primera)

---

## 🚀 Cómo Usar

### Agrupación Automática - Ejemplo Práctico

**Escenario:** Tienes abiertas:
- 3 ventanas de navegador (Chrome/Firefox)
- 2 ventanas de VS Code
- 1 ventana de Terminal
- 2 ventanas de Slack

**Pasos:**

1. **Abrir WindowTabsFree**
   ```bash
   dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
   ```

2. **Hacer clic en "Auto-Group by App"** (botón verde en la barra superior)

3. **Resultado:** Se crean automáticamente 3 grupos:
   - 🔵 "chrome Windows" (3 ventanas)
   - 🟢 "Code Windows" (2 ventanas)
   - 🟡 "slack Windows" (2 ventanas)
   
   La ventana de Terminal NO se agrupa (solo tiene 1 ventana)

4. **Navegar entre pestañas:**
   - Selecciona el grupo "chrome Windows"
   - Haz clic en "Next Tab ►" para cambiar entre las 3 ventanas de Chrome
   - Cada clic enfoca la siguiente ventana
   - El indicador muestra "Tab 1 of 3", "Tab 2 of 3", etc.

---

## 🎨 Panel de Grupos

Cada grupo muestra:

```
┌─────────────────────────────┐
│ chrome Windows          [×] │ ← Nombre y botón eliminar
│ App: chrome                 │ ← Nombre de aplicación (auto-grupos)
│ Windows: 3                  │ ← Cantidad de ventanas
│ [◄ Prev Tab] [Next Tab ►]  │ ← Navegación
│                             │
│ Active Tab:                 │
│ Tab 2 of 3                  │ ← Indicador visual (verde)
└─────────────────────────────┘
```

---

## ⌨️ Navegación con Teclado (Futuro)

Las siguientes teclas están planificadas para una futura actualización:

- `Ctrl+Tab` - Siguiente pestaña en el grupo activo
- `Ctrl+Shift+Tab` - Pestaña anterior en el grupo activo
- `Alt+1-5` - Cambiar a ventana específica
- `Alt+→` - Siguiente ventana (todas)
- `Alt+←` - Ventana anterior (todas)

**Nota:** Actualmente usar los botones "Next Tab" y "Prev Tab" en la UI.

---

## 🔧 Gestión de Grupos

### Crear Grupo Manual
1. Clic en "New Tab Group"
2. Ingresa un nombre
3. Selecciona el grupo creado
4. Haz clic en "Add to Group" junto a ventanas

### Eliminar Grupos
- **Un grupo específico:** Clic en [×] junto al nombre del grupo
- **Todos los auto-grupos:** Clic en "Remove Auto-Groups" (botón naranja)

### Reorganizar
Los grupos se listan en el orden que fueron creados. Los grupos automáticos se agregan al final.

---

## 💡 Consejos

1. **Actualización Automática:** La lista de ventanas se actualiza cada 2 segundos
   - Si cierras una ventana, desaparece automáticamente
   - Si abres una nueva, aparece en la lista

2. **Colores de Grupos:** Cada aplicación auto-agrupada recibe un color único
   - Basado en hash del nombre de la app
   - Ayuda a identificar visualmente los grupos

3. **Grupos Persistentes:** Los grupos se guardan automáticamente
   - Se mantienen entre sesiones
   - Ubicación: `~/.config/WindowTabsFree/settings.json`

4. **Re-agrupar:** Puedes hacer clic en "Auto-Group by App" múltiples veces
   - Actualiza los grupos existentes
   - Agrega nuevas ventanas a grupos existentes

---

## 📋 Casos de Uso

### Desarrollo de Software
```
Grupos automáticos típicos:
- VS Code Windows (editor)
- Terminal Windows (consolas)
- chrome Windows (navegador/docs)
- Postman Windows (API testing)
```

### Diseño Gráfico
```
Grupos automáticos típicos:
- Photoshop Windows (imágenes)
- Illustrator Windows (vectores)
- Finder/Explorer Windows (archivos)
```

### Multitarea General
```
Organiza automáticamente:
- Múltiples documentos de la misma app
- Varias ventanas de chat
- Diferentes hojas de cálculo
```

---

## 🐛 Solución de Problemas

**P: No veo el botón "Auto-Group by App"**
R: Asegúrate de estar usando la última versión. Verifica la barra superior.

**P: Al hacer clic en "Next Tab" no pasa nada**
R: Verifica que el grupo tenga al menos 1 ventana. El contador debe mostrar "Windows: X" con X > 0.

**P: Las ventanas desaparecen de los grupos**
R: Si cierras una ventana, se elimina automáticamente del grupo. Esto es normal.

**P: ¿Puedo cambiar el color de un grupo?**
R: Actualmente los colores se asignan automáticamente. En el futuro se podrá personalizar.

---

## 🔄 Flujo de Trabajo Recomendado

1. **Inicio del día:**
   - Abre WindowTabsFree
   - Haz clic en "Auto-Group by App"
   - Revisa los grupos creados

2. **Durante el trabajo:**
   - Usa "Next Tab" / "Prev Tab" para navegar
   - Agrega manualmente ventanas que necesites en grupos específicos
   - Usa "Refresh Now" si la lista no se actualiza

3. **Reorganización:**
   - "Remove Auto-Groups" para limpiar
   - "Auto-Group by App" para re-agrupar
   - Crea grupos manuales para flujos de trabajo específicos

---

## 📚 Documentación Adicional

- **README_NET8.md** - Guía completa de instalación y configuración
- **QUICKSTART_ES.md** - Guía rápida de ejecución
- **IMPLEMENTATION_SUMMARY.md** - Detalles técnicos de implementación

---

## 🎉 ¡Disfruta la Organización!

Con estas nuevas funcionalidades, gestionar múltiples ventanas de las mismas aplicaciones es mucho más fácil. 

**¿Preguntas o sugerencias?** Abre un issue en GitHub.
