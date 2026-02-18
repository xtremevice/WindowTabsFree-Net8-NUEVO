# Guía de Configuración de Hotkeys en Linux

## Resumen

WindowTabsFree en Linux usa **evdev** (interfaz de dispositivos de eventos) para capturar hotkeys globales de teclado. Esto requiere permisos especiales para leer desde los dispositivos `/dev/input`.

## ¿Por qué evdev?

- **Funciona en X11 y Wayland**: A diferencia de soluciones específicas de X11, evdev funciona en ambos servidores de visualización
- **Bajo nivel**: Lee eventos de teclado directamente desde el kernel
- **Confiable**: No depende del entorno de escritorio o gestor de ventanas

## Requisitos de Permisos

Para usar hotkeys globales en Linux, necesitas acceso de lectura a los dispositivos `/dev/input/event*`.

### Opción 1: Agregar Usuario al Grupo 'input' (Recomendado)

Este es el enfoque recomendado ya que no requiere privilegios de root para ejecutar la aplicación.

1. **Agregar tu usuario al grupo input:**
   ```bash
   sudo usermod -a -G input $USER
   ```

2. **Cerrar sesión y volver a iniciar sesión** (o reiniciar)
   - Esto es necesario para que la membresía al grupo tome efecto
   - Simplemente abrir una nueva terminal no funcionará

3. **Verificar membresía al grupo:**
   ```bash
   groups
   ```
   Deberías ver `input` en la lista

4. **Ejecutar WindowTabsFree:**
   ```bash
   dotnet run
   ```
   O ejecutar el ejecutable publicado normalmente

### Opción 2: Ejecutar con sudo (No Recomendado)

Ejecutar toda la aplicación con sudo es un riesgo de seguridad y no se recomienda:

```bash
sudo dotnet run
```

⚠️ **Advertencia de Seguridad**: Ejecutar aplicaciones GUI con sudo puede ser peligroso y generalmente se desaconseja.

### Opción 3: Crear Regla udev (Avanzado)

Para un control más fino, puedes crear una regla udev:

1. **Crear archivo de regla udev:**
   ```bash
   sudo nano /etc/udev/rules.d/99-input.rules
   ```

2. **Agregar esta línea:**
   ```
   KERNEL=="event*", SUBSYSTEM=="input", MODE="0660", GROUP="input"
   ```

3. **Recargar reglas udev:**
   ```bash
   sudo udevadm control --reload-rules
   sudo udevadm trigger
   ```

4. **Agregar tu usuario al grupo input** (como en la Opción 1)

## Solución de Problemas

### Los Hotkeys No Funcionan

**1. Verificar si estás en el grupo input:**
```bash
groups | grep input
```

**2. Verificar permisos de /dev/input:**
```bash
ls -l /dev/input/event*
```

Deberías ver algo como:
```
crw-rw---- 1 root input 13, 64 Feb 17 10:00 /dev/input/event0
```

El grupo `input` debe tener permisos de lectura (rw-).

**3. Verificar salida de consola:**

La aplicación registra mensajes útiles:
```
[Linux/evdev] Starting keyboard event monitoring
[Linux/evdev] Found 2 keyboard device(s)
[Linux/evdev] Monitoring: /dev/input/event2
```

**Si ves errores de permisos:**
```
[Linux/evdev] ⚠️  No keyboard devices found or no permission to access them
[Linux/evdev] Make sure you're in the 'input' group (logout/login required after adding)
```

Esto significa que necesitas agregarte al grupo input y cerrar/iniciar sesión.

### Encontrar Dispositivos de Teclado

El servicio busca automáticamente dispositivos de teclado en:
1. `/dev/input/by-id/` - Dispositivos con "kbd" o "keyboard" en el nombre
2. `/dev/input/event*` - Todos los dispositivos de eventos

Puedes verificar manualmente qué dispositivos son teclados:
```bash
cat /proc/bus/input/devices | grep -A 5 keyboard
```

### Probar Registro de Hotkeys

Cuando configures hotkeys en la aplicación, deberías ver:
```
[Linux/evdev] Attempting to register hotkey: Ctrl+Alt+Right
[Linux/evdev] Successfully registered hotkey: Ctrl+Alt+Right
```

Cuando presiones el hotkey:
```
[Linux/evdev] Hotkey triggered: Ctrl+Alt+Right
```

## Teclas Soportadas

### Modificadores
- **Ctrl** / **Control**
- **Alt**
- **Shift**
- **Win** / **Super** / **Meta** (tecla Windows/Super)

### Teclas de Letras
- A-Z (no distingue mayúsculas)

### Teclas Numéricas
- 0-9 (fila superior)

### Teclas de Función
- F1-F12

### Teclas de Flecha
- Left, Right, Up, Down

### Teclas Especiales
- Space
- Enter
- Esc / Escape
- Tab
- Backspace

### Ejemplos

Combinaciones de hotkeys válidas:
- `Ctrl+Alt+Right` - Ctrl + Alt + Flecha Derecha
- `Ctrl+Shift+A` - Ctrl + Shift + A
- `Win+F1` - Windows/Super + F1
- `Alt+Tab` - Alt + Tab

## Cómo Funciona

1. **Descubrimiento de Dispositivos**: El servicio encuentra dispositivos de teclado en `/dev/input`
2. **Lectura de Eventos**: Abre archivos de dispositivos y lee eventos de entrada sin procesar
3. **Análisis de Eventos**: Analiza estructuras `input_event` de evdev (24 bytes cada una)
4. **Seguimiento de Teclas**: Rastrea qué teclas están presionadas actualmente
5. **Coincidencia de Hotkeys**: Cuando se suelta una tecla, verifica si la combinación coincide con algún hotkey registrado
6. **Ejecución de Callback**: Ejecuta el callback para hotkeys coincidentes

## Consideraciones de Seguridad

- El grupo input otorga acceso de lectura a **todos** los dispositivos de entrada (teclado, mouse, touchpad)
- Esto significa que la aplicación puede leer todas las entradas de teclado en todo el sistema
- Solo agregar usuarios confiables al grupo input
- La aplicación solo lee eventos cuando hay hotkeys registrados
- No se produce registro de teclas ni registro de pulsaciones de teclas que no sean hotkeys

## Notas de Plataforma

### Funciona En
- ✅ Ubuntu / Debian
- ✅ Fedora / RHEL / CentOS
- ✅ Arch Linux
- ✅ La mayoría de distribuciones modernas de Linux
- ✅ X11 y Wayland

### Requisitos
- Kernel de Linux con soporte evdev (prácticamente todos los kernels modernos)
- Acceso de lectura a dispositivos `/dev/input`
- .NET 8.0 runtime

## Soluciones Alternativas

Si no puedes o no quieres otorgar acceso a dispositivos de entrada:

1. **Usar Botones de UI**: La aplicación proporciona botones de UI para todas las acciones de hotkeys
2. **Atajos del Entorno de Escritorio**: Configura tu DE para ejecutar comandos:
   ```bash
   # Ejemplo para atajos personalizados de GNOME/KDE
   dotnet run --project WindowTabsFree.UI -- --next-window
   ```
3. **X11 XGrabKey**: Podríamos implementar hotkeys específicos de X11 (solo X11, no Wayland)

## Mejoras Futuras

Posibles mejoras:
- Soporte para más códigos de teclas
- Filtrado de dispositivos configurable
- Detección/exclusión de teclado virtual
- Manejo de repetición de teclas
- Soporte para acordes de múltiples teclas

## Obtener Ayuda

Si todavía tienes problemas:

1. Verifica la salida de consola para mensajes de error
2. Verifica membresía al grupo: `groups | grep input`
3. Verifica permisos de dispositivos: `ls -l /dev/input/event*`
4. Intenta ejecutar con sudo una vez para probar (solo para verificar que funciona con permisos)
5. Reporta problemas con la salida de consola incluida

## Referencias

- [Subsistema de Entrada de Linux](https://www.kernel.org/doc/html/latest/input/input.html)
- [API evdev](https://www.kernel.org/doc/html/latest/input/event-codes.html)
- [Permisos de Dispositivos de Entrada](https://wiki.archlinux.org/title/Users_and_groups#Pre-systemd_groups)
