# Guía de Configuración de Hotkeys en macOS

## Descripción General

WindowTabsFree utiliza atajos de teclado globales para cambiar entre ventanas. En macOS, los atajos globales requieren **permisos de Accesibilidad** para funcionar correctamente.

## Verificación de Permisos al Iniciar

Cuando inicias WindowTabsFree en macOS, automáticamente verifica si tiene permisos de Accesibilidad:

- **Si los permisos están concedidos:** ✓ Los atajos funcionarán inmediatamente
- **Si los permisos NO están concedidos:** ✗ Verás instrucciones en la consola sobre cómo habilitarlos

**Importante:** A diferencia de algunas aplicaciones, WindowTabsFree no puede mostrar automáticamente el diálogo de permisos del sistema. Debes habilitar los permisos manualmente en Configuración del Sistema.

## Cómo Habilitar Permisos de Accesibilidad

Si ves este mensaje en la consola:
```
[macOS] ✗ Accessibility permissions NOT granted
[macOS] HOTKEYS WILL NOT WORK without Accessibility permissions!
```

Sigue estos pasos:

### Paso 1: Abrir Configuración del Sistema

1. Haz clic en el menú Apple () en la esquina superior izquierda
2. Selecciona **Configuración del Sistema** (o **Preferencias del Sistema** en versiones antiguas de macOS)

### Paso 2: Navegar a Privacidad y Seguridad

1. En Configuración del Sistema, haz clic en **Privacidad y Seguridad** en la barra lateral
2. Desplázate hacia abajo y haz clic en **Accesibilidad**

### Paso 3: Otorgar Permiso a WindowTabsFree

1. Haz clic en el icono de candado 🔒 en la parte inferior para realizar cambios (puede que necesites ingresar tu contraseña)
2. Haz clic en el botón **+** para agregar una aplicación
3. Navega hasta donde está instalado WindowTabsFree (generalmente `/Applications` o tu carpeta de usuario)
4. Selecciona **WindowTabsFree** y haz clic en **Abrir**
5. Asegúrate de que la casilla junto a WindowTabsFree esté **habilitada** ✓

### Paso 4: Reiniciar WindowTabsFree

1. Cierra WindowTabsFree completamente
2. Reinicia la aplicación
3. ¡Los atajos de teclado deberían funcionar ahora!

## Alternativa: Usando la Línea de Comandos

Si te sientes cómodo con la terminal, puedes verificar si WindowTabsFree tiene permisos de accesibilidad:

```bash
# Verificar si la app tiene permisos de accesibilidad
sqlite3 /Library/Application\ Support/com.apple.TCC/TCC.db \
  "SELECT * FROM access WHERE service='kTCCServiceAccessibility';"
```

## Referencia de Códigos de Error

| Código de Error | Significado | Solución |
|-----------------|-------------|----------|
| -50 | Error de parámetro / Permisos faltantes | Habilitar permisos de Accesibilidad |
| -9999 | Atajo ya registrado | Probar una combinación de teclas diferente |

## Solución de Problemas

### ¿Los Atajos Siguen Sin Funcionar?

1. **Verifica la combinación de teclas**: Asegúrate de que tu atajo no entre en conflicto con atajos del sistema
2. **Prueba diferentes teclas**: Usa combinaciones como:
   - `Ctrl+Alt+Right` (Siguiente ventana)
   - `Ctrl+Alt+Left` (Ventana anterior)
   - `Cmd+Shift+Right` (Estilo macOS)
3. **Reinicia tu Mac**: A veces los permisos necesitan un reinicio completo para tomar efecto
4. **Elimina y vuelve a agregar**: En la configuración de Accesibilidad, elimina WindowTabsFree y agrégalo de nuevo

### macOS Ventura (13.0+) y Posteriores

En versiones más nuevas de macOS, puede que necesites:
1. Otorgar **Acceso Completo al Disco** además de Accesibilidad
2. Permitir la app en **Seguridad y Privacidad** → **Privacidad** → **Monitoreo de Entrada**

## Atajos Predeterminados

La configuración predeterminada de atajos:
- **Siguiente Ventana**: `Ctrl+Alt+Right`
- **Ventana Anterior**: `Ctrl+Alt+Left`

Puedes cambiarlos en la configuración de la aplicación.

## Limitaciones del Sistema

- Algunas aplicaciones del sistema (como Configuración del Sistema, Finder) pueden no responder al cambio de ventanas
- Las aplicaciones de `/System/Library/CoreServices/` se excluyen automáticamente de la gestión
- Los procesos en segundo plano y las apps de la barra de menú típicamente no tienen ventanas manejables

## ¿Necesitas Ayuda?

Si continúas experimentando problemas:
1. Verifica la consola de la aplicación para mensajes de error detallados
2. Reporta el problema en GitHub con el código de error y la versión de macOS
3. Incluye capturas de pantalla de tu configuración de Privacidad y Seguridad

## Documentación Relacionada

- [README Principal](README.md)
- [Exclusiones de Apps del Sistema en macOS](SYSTEM_EXCLUSIONS_ES.md)
- [Guía General de Hotkeys](HOTKEYS_ES.md)
