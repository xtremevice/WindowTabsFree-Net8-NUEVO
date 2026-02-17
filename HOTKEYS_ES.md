# Soporte de Atajos Globales (Hotkeys)

Este documento describe la función de atajos globales que permite cambiar entre ventanas incluso cuando la aplicación no está enfocada.

## Resumen

La aplicación ahora soporta atajos globales que funcionan a nivel de sistema, permitiéndote cambiar entre ventanas sin tener que enfocar primero la aplicación WindowTabsFree.

## Atajos Predeterminados

Los siguientes atajos están configurados por defecto:

- **Siguiente Ventana**: `Ctrl+Alt+Right` - Cambia a la siguiente ventana manejable
- **Ventana Anterior**: `Ctrl+Alt+Left` - Cambia a la ventana anterior manejable

## Configurar Atajos

### Via Archivo de Configuración

Edita el archivo `settings.json` (típicamente ubicado en el directorio de configuración de la aplicación):

```json
{
  "HotKeys": {
    "NextWindow": "Ctrl+Alt+Right",
    "PreviousWindow": "Ctrl+Alt+Left"
  }
}
```

### Via Interfaz (si está disponible)

1. Abre la aplicación
2. Busca la opción "Configure Hotkeys" en el Administrador de Ventana Flotante
3. Haz clic en el cuadro de texto del atajo que deseas configurar
4. Presiona la combinación de teclas deseada
5. Haz clic en "Save"

## Combinaciones de Teclas Soportadas

El analizador de atajos soporta los siguientes modificadores:
- `Ctrl` - Tecla Control
- `Alt` - Tecla Alt
- `Shift` - Tecla Shift
- `Win` - Tecla Windows/Super

Y las siguientes teclas:
- Letras: A-Z
- Números: 0-9
- Teclas de función: F1-F12
- Teclas de flecha: Left, Right, Up, Down
- Teclas especiales: Space, Enter, Esc, Tab, Backspace, Delete, Insert, Home, End, PageUp, PageDown
- Teclas de NumPad: NumPad0-NumPad9, NumPad+, NumPad-, NumPad*, NumPad/

### Ejemplos

- `Ctrl+Alt+A`
- `Ctrl+Shift+T`
- `Win+R`
- `Alt+F4`
- `Ctrl+F1`

## Cómo Funciona

1. **Windows**: Usa la función API de Windows `RegisterHotKey` para registrar atajos globales
2. **macOS**: Aún no implementado (existe un placeholder)
3. **Linux**: Aún no implementado (existe un placeholder)

### Detalles Técnicos

El servicio de atajos:
- Se ejecuta en un hilo en segundo plano con una ventana de solo mensajes (Windows)
- Registra atajos globalmente a nivel del sistema operativo
- Invoca callbacks cuando se presionan los atajos
- Desregistra automáticamente los atajos cuando se cierra la aplicación

### Ciclo de Ventanas

Cuando presionas un atajo:
1. La aplicación obtiene la lista de todas las ventanas manejables (excluyendo ventanas del sistema y exclusiones configuradas)
2. Identifica la ventana actualmente enfocada
3. Cambia a la siguiente/anterior ventana en la lista
4. La lista se enrolla (después de la última ventana, va a la primera)

## Solución de Problemas

### Los Atajos No Funcionan

1. **Verifica si el atajo ya está registrado**: Otra aplicación podría estar usando la misma combinación de teclas. Prueba con una combinación diferente.
2. **Solo Windows**: La implementación completa actualmente solo funciona en Windows. El soporte para macOS y Linux se agregará en futuras actualizaciones.
3. **Revisa la salida de consola**: La aplicación registra cuando se registran los atajos. Busca mensajes de error.
4. **Reinicia la aplicación**: Los cambios en la configuración de atajos requieren un reinicio de la aplicación para tener efecto.

### Conflictos con Otras Aplicaciones

Si tu atajo no funciona, podría estar en conflicto con:
- Atajos del sistema (ej. Win+L, Ctrl+Alt+Delete)
- Otras aplicaciones que han registrado el mismo atajo
- Prueba usar una combinación más única con múltiples modificadores

## Soporte de Plataformas

| Plataforma | Estado | Notas |
|------------|--------|-------|
| Windows | ✅ Totalmente Soportado | Usa API RegisterHotKey |
| macOS | ⚠️ Placeholder | Requiere implementación de Carbon/Cocoa EventTap |
| Linux | ⚠️ Placeholder | Requiere implementación de X11 XGrabKey o DBus |

## Mejoras Futuras

- Recarga en caliente de configuración de atajos sin reiniciar
- Selección de ventanas más granular (por aplicación, por grupo, etc.)
- Detección y advertencias de conflictos de atajos
- Retroalimentación visual cuando se presiona un atajo
- Soporte completo para macOS y Linux
