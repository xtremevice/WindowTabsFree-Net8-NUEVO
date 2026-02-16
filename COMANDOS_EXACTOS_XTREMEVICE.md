# Comandos Exactos para Migración - xtremevice

## 🎯 Comandos Personalizados Listos para Usar

Este documento contiene los comandos EXACTOS para migrar WindowTabsFree-Net8 a un repositorio separado, con toda tu configuración específica ya incluida.

**NO necesitas reemplazar nada** - simplemente copia y pega los bloques de comandos.

---

## Información de tu Configuración

- **Usuario GitHub:** xtremevice
- **Proyecto actual:** WindowTabsFree
- **Ubicación actual:** `~/Projects/WindowTabsFree`
- **Carpeta destino:** `~/Projects`
- **Nuevo repositorio:** WindowTabsFree-Net8
- **URL del repositorio:** https://github.com/xtremevice/WindowTabsFree-Net8.git
- **Branch actual:** copilot/implement-window-service-functionality

---

## Requisitos Previos

Antes de comenzar, verifica que tienes:

```bash
# Verificar .NET 8 SDK
dotnet --version  # Debe ser 8.0.x

# Verificar Git
git --version  # Cualquier versión moderna

# Verificar que estás en el directorio correcto
cd ~/Projects/WindowTabsFree
pwd  # Debe mostrar: /home/TU_USUARIO/Projects/WindowTabsFree
```

---

## PASO 1: Actualizar Repositorio Actual

Descarga todos los cambios más recientes del branch actual:

```bash
cd ~/Projects/WindowTabsFree
git checkout copilot/implement-window-service-functionality
git pull origin copilot/implement-window-service-functionality
```

**Verificar:**
```bash
git log -1 --oneline
# Debe mostrar el commit más reciente
```

---

## PASO 2: Crear Nuevo Repositorio en GitHub

### Opción A: Via Web (Recomendado)

1. Ir a: https://github.com/new
2. **Owner:** xtremevice
3. **Repository name:** WindowTabsFree-Net8
4. **Description:** WindowTabsFree .NET 8 Cross-Platform Implementation
5. **Visibility:** Public (o Private según prefieras)
6. **NO marcar** "Initialize this repository with a README"
7. Click "Create repository"

### Opción B: Via CLI (si tienes GitHub CLI instalado)

```bash
gh repo create xtremevice/WindowTabsFree-Net8 \
  --public \
  --description "WindowTabsFree .NET 8 Cross-Platform Implementation"
```

---

## PASO 3: Crear Directorio y Copiar Contenido

Crea el directorio en `~/Projects` y copia todo el contenido:

```bash
# Crear directorio para el nuevo repositorio
mkdir -p ~/Projects/WindowTabsFree-Net8

# Entrar al nuevo directorio
cd ~/Projects/WindowTabsFree-Net8

# Copiar TODO el contenido de WindowTabsFree-Net8
cp -r ~/Projects/WindowTabsFree/WindowTabsFree-Net8/* .

# Copiar también el .gitignore (archivo oculto)
cp ~/Projects/WindowTabsFree/WindowTabsFree-Net8/.gitignore .
```

**Verificar que se copió todo:**
```bash
ls -la
# Debe mostrar: src/, *.md, .gitignore, etc.

# Contar archivos
find . -type f | wc -l
# Debe mostrar ~250+ archivos
```

---

## PASO 4: Inicializar Git en el Nuevo Directorio

```bash
# Asegurarte de estar en el directorio correcto
cd ~/Projects/WindowTabsFree-Net8

# Inicializar repositorio Git
git init

# Agregar todos los archivos
git add .

# Verificar qué se agregará
git status

# Crear commit inicial
git commit -m "Initial commit: WindowTabsFree .NET 8 implementation

Complete cross-platform implementation with:
- Windows, Linux, and macOS support
- Auto-grouping by application  
- Floating window manager
- Tab visualization and navigation
- 23 documentation files in Spanish and English
- Build scripts for all platforms"
```

**Verificar:**
```bash
git log -1
# Debe mostrar tu commit inicial
```

---

## PASO 5: Conectar con GitHub y Push

```bash
# Conectar con tu nuevo repositorio
git remote add origin https://github.com/xtremevice/WindowTabsFree-Net8.git

# Cambiar a rama main
git branch -M main

# Push inicial
git push -u origin main
```

**Verificar:**
```bash
git remote -v
# Debe mostrar:
# origin  https://github.com/xtremevice/WindowTabsFree-Net8.git (fetch)
# origin  https://github.com/xtremevice/WindowTabsFree-Net8.git (push)
```

---

## PASO 6: Verificar que Todo Funciona

```bash
# Verificar configuración de Git
cd ~/Projects/WindowTabsFree-Net8
git remote -v
git branch -vv

# Verificar que compila
dotnet build WindowTabsFreeNet.sln
# Debe mostrar: Build succeeded
```

---

## PASO 7: Trabajar desde el Nuevo Repositorio

Desde ahora, trabaja SOLO en el nuevo repositorio:

```bash
# Siempre trabajar aquí
cd ~/Projects/WindowTabsFree-Net8

# Abrir en tu editor favorito
code .  # VS Code
# o
rider .  # JetBrains Rider

# Workflow normal de Git
git add .
git commit -m "Nueva feature"
git push
```

---

## 🚀 Bloques Unificados (Copy-Paste)

Para ejecutar más rápido, usa estos bloques que combinan múltiples comandos:

### Bloque 1 - Actualizar Original (1 minuto)

```bash
cd ~/Projects/WindowTabsFree && \
git checkout copilot/implement-window-service-functionality && \
git pull origin copilot/implement-window-service-functionality
```

### Bloque 2 - Copiar y Setup (2 minutos)

```bash
mkdir -p ~/Projects/WindowTabsFree-Net8 && \
cd ~/Projects/WindowTabsFree-Net8 && \
cp -r ~/Projects/WindowTabsFree/WindowTabsFree-Net8/* . && \
cp ~/Projects/WindowTabsFree/WindowTabsFree-Net8/.gitignore . && \
git init && \
git add . && \
git commit -m "Initial commit: WindowTabsFree .NET 8 implementation"
```

### Bloque 3 - Push (1 minuto)

**IMPORTANTE:** Ejecutar DESPUÉS de crear el repositorio en GitHub.

```bash
git remote add origin https://github.com/xtremevice/WindowTabsFree-Net8.git && \
git branch -M main && \
git push -u origin main
```

---

## ✅ Comandos de Verificación

Usa estos comandos para verificar cada paso:

```bash
# Verificar que el repo original está limpio
cd ~/Projects/WindowTabsFree
git status  # Debe decir "nothing to commit, working tree clean"

# Verificar que el nuevo repo está correcto
cd ~/Projects/WindowTabsFree-Net8
git remote -v  # Debe mostrar xtremevice/WindowTabsFree-Net8
git log --oneline  # Debe mostrar tu commit
git branch -vv  # Debe mostrar main tracking origin/main

# Verificar que compila
dotnet build WindowTabsFreeNet.sln  # Debe ser exitoso
```

---

## 🏗️ Estructura Final

Después de completar la migración, tendrás esta estructura:

```
~/Projects/
├── WindowTabsFree/                    # Repositorio ORIGINAL (intacto)
│   ├── .git/                          # Git del original
│   ├── WindowTabsFree-Net8/           # Subdirectorio (de donde copiamos)
│   │   ├── src/
│   │   ├── WindowTabsFreeNet.sln
│   │   └── ...documentación...
│   ├── src/                           # Código legacy (F#, etc.)
│   ├── WindowTabs.sln                 # Solución original
│   └── ...otros archivos...
│
└── WindowTabsFree-Net8/               # Nuevo REPOSITORIO (independiente)
    ├── .git/                          # Git NUEVO e independiente
    ├── src/
    │   ├── WindowTabsFree.Common/
    │   ├── WindowTabsFree.Core/
    │   ├── WindowTabsFree.Services/
    │   ├── WindowTabsFree.UI/
    │   └── WindowTabsFree.TestConsole/
    ├── WindowTabsFreeNet.sln
    ├── README.md
    ├── LEEME_PRIMERO.md
    └── ...22 archivos de documentación...
```

---

## 🔧 Troubleshooting

### Error: Permission denied (publickey)

```bash
# Cambiar a HTTPS en lugar de SSH
cd ~/Projects/WindowTabsFree-Net8
git remote set-url origin https://github.com/xtremevice/WindowTabsFree-Net8.git
git push -u origin main
```

### Error: Repository not found

```bash
# Verificar que creaste el repositorio en GitHub
# Ir a: https://github.com/xtremevice/WindowTabsFree-Net8

# Verificar la URL
cd ~/Projects/WindowTabsFree-Net8
git remote -v
```

### Error: Failed to push some refs

```bash
# Si el repo en GitHub no está vacío, pull primero
cd ~/Projects/WindowTabsFree-Net8
git pull origin main --allow-unrelated-histories
git push -u origin main
```

### El directorio ~/Projects no existe

```bash
# Crear el directorio primero
mkdir -p ~/Projects
cd ~/Projects
# Luego seguir con los pasos normales
```

---

## 📝 Notas Importantes

1. **NO reemplaces nada** - Todos los comandos ya tienen tu información correcta
2. **Ejecuta en orden** - Sigue los pasos del 1 al 7
3. **El repo original NO se modifica** - Solo se copia, no se mueve
4. **Después de PASO 7, trabaja SOLO en `~/Projects/WindowTabsFree-Net8`**
5. **Los dos repositorios son COMPLETAMENTE independientes**

---

## ⏱️ Tiempo Estimado

- **Paso 1:** Actualizar original → 1 minuto
- **Paso 2:** Crear repo en GitHub → 2 minutos
- **Paso 3:** Copiar archivos → 1 minuto
- **Paso 4:** Git init y commit → 1 minuto
- **Paso 5:** Push al repositorio → 1 minuto
- **Paso 6:** Verificar → 30 segundos

**TOTAL: ~6-7 minutos**

---

## 🎉 ¡Listo!

Después de completar estos pasos:

✅ Tienes un repositorio completamente independiente en https://github.com/xtremevice/WindowTabsFree-Net8  
✅ El código original en `~/Projects/WindowTabsFree` permanece intacto  
✅ Puedes trabajar en `~/Projects/WindowTabsFree-Net8` sin afectar el original  
✅ Puedes compartir el nuevo repositorio fácilmente  
✅ Puedes hacer releases independientes  
✅ Tienes toda la documentación (23 archivos)  

---

## 📚 Próximos Pasos

Una vez completada la migración:

1. **Configurar CI/CD:**
   ```bash
   cd ~/Projects/WindowTabsFree-Net8
   # Ver documentación en README.md
   ```

2. **Invitar colaboradores:**
   - Ir a: https://github.com/xtremevice/WindowTabsFree-Net8/settings/access
   - Click "Add people"

3. **Crear primera release:**
   ```bash
   cd ~/Projects/WindowTabsFree-Net8
   git tag v1.0.0
   git push origin v1.0.0
   ```

4. **Continuar desarrollo:**
   ```bash
   cd ~/Projects/WindowTabsFree-Net8
   git checkout -b feature/nueva-funcionalidad
   # ...hacer cambios...
   git add .
   git commit -m "Nueva funcionalidad"
   git push -u origin feature/nueva-funcionalidad
   ```

---

## 🔗 Enlaces Útiles

- **Tu nuevo repositorio:** https://github.com/xtremevice/WindowTabsFree-Net8
- **Documentación principal:** `~/Projects/WindowTabsFree-Net8/LEEME_PRIMERO.md`
- **Guía de uso:** `~/Projects/WindowTabsFree-Net8/COMO_USAR_ESTE_PROYECTO.md`
- **Guía de agrupación:** `~/Projects/WindowTabsFree-Net8/GUIA_AGRUPACION.md`

---

**¡Éxito con tu nuevo repositorio WindowTabsFree-Net8!** 🚀
