#!/bin/bash

# WindowTabsFree - macOS Apple Silicon (ARM64) Build Script
# Para Mac M1, M2, M3 y chips Apple Silicon posteriores

set -e  # Salir si hay error

echo "======================================"
echo "WindowTabsFree - macOS ARM64 Builder"
echo "======================================"
echo ""

# Colores para output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Verificar que estamos en el directorio correcto
if [ ! -f "WindowTabsFreeNet.sln" ]; then
    echo -e "${RED}❌ Error: No se encontró WindowTabsFreeNet.sln${NC}"
    echo "   Por favor ejecuta este script desde el directorio raíz del proyecto"
    exit 1
fi

# Verificar que tenemos .NET 8
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ Error: dotnet no está instalado${NC}"
    echo "   Instala .NET 8 SDK desde: https://dotnet.microsoft.com/download"
    exit 1
fi

echo -e "${BLUE}ℹ️  Versión de .NET:${NC}"
dotnet --version
echo ""

# Verificar que es Apple Silicon
ARCH=$(uname -m)
if [ "$ARCH" != "arm64" ]; then
    echo -e "${YELLOW}⚠️  Advertencia: No estás en Apple Silicon (detectado: $ARCH)${NC}"
    echo "   Este script está optimizado para Mac M1/M2/M3"
    echo "   ¿Continuar de todas formas? (y/n)"
    read -r response
    if [[ ! "$response" =~ ^[Yy]$ ]]; then
        exit 0
    fi
fi

# Configuración
PROJECT_PATH="src/WindowTabsFree.UI/WindowTabsFree.UI.csproj"
OUTPUT_DIR="publish/macos-arm64"
RUNTIME="osx-arm64"

echo -e "${BLUE}📦 Configuración:${NC}"
echo "   Proyecto: $PROJECT_PATH"
echo "   Runtime: $RUNTIME (Apple Silicon)"
echo "   Salida: $OUTPUT_DIR"
echo ""

# Limpiar publicaciones anteriores
if [ -d "$OUTPUT_DIR" ]; then
    echo -e "${YELLOW}🧹 Limpiando publicación anterior...${NC}"
    rm -rf "$OUTPUT_DIR"
fi

# Publicar aplicación
echo -e "${BLUE}🔨 Compilando aplicación para macOS ARM64...${NC}"
echo ""

dotnet publish "$PROJECT_PATH" \
    --runtime "$RUNTIME" \
    --self-contained true \
    --configuration Release \
    --output "$OUTPUT_DIR" \
    /p:PublishSingleFile=false \
    /p:PublishReadyToRun=true

# Verificar que la compilación fue exitosa
if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ ¡Compilación exitosa!${NC}"
    echo ""
    
    # Información del ejecutable
    EXECUTABLE="$OUTPUT_DIR/WindowTabsFree.UI"
    
    if [ -f "$EXECUTABLE" ]; then
        # Hacer ejecutable
        chmod +x "$EXECUTABLE"
        
        # Obtener tamaño
        SIZE=$(du -sh "$OUTPUT_DIR" | cut -f1)
        
        echo -e "${GREEN}📊 Información del ejecutable:${NC}"
        echo "   📁 Ubicación: $EXECUTABLE"
        echo "   💾 Tamaño total: $SIZE"
        echo "   🏗️  Arquitectura: ARM64 (Apple Silicon)"
        echo ""
        
        echo -e "${BLUE}🚀 Para ejecutar:${NC}"
        echo "   ./$EXECUTABLE"
        echo ""
        echo -e "${BLUE}📂 O desde Finder:${NC}"
        echo "   1. Abre Finder"
        echo "   2. Ve a: $(pwd)/$OUTPUT_DIR"
        echo "   3. Doble clic en: WindowTabsFree.UI"
        echo ""
        
        echo -e "${YELLOW}⚠️  IMPORTANTE - Permisos de macOS:${NC}"
        echo "   Si macOS bloquea la ejecución:"
        echo "   1. Ve a Preferencias del Sistema → Seguridad y Privacidad"
        echo "   2. En la pestaña 'General', haz clic en 'Abrir de todas formas'"
        echo ""
        echo "   La aplicación necesita permisos de Accesibilidad:"
        echo "   1. Ve a Preferencias del Sistema → Seguridad y Privacidad → Privacidad"
        echo "   2. Selecciona 'Accesibilidad'"
        echo "   3. Agrega WindowTabsFree.UI y marca la casilla"
        echo ""
        
        echo -e "${GREEN}✨ ¡Listo! Tu aplicación está compilada y lista para usar.${NC}"
        echo ""
        echo -e "${BLUE}📚 Más información:${NC}"
        echo "   Guía completa: GUIA_MAC_APPLE_SILICON.md"
        echo "   Guía macOS general: MACOS_GUIDE.md"
        echo ""
        
    else
        echo -e "${RED}❌ Error: No se encontró el ejecutable en $EXECUTABLE${NC}"
        exit 1
    fi
else
    echo ""
    echo -e "${RED}❌ Error durante la compilación${NC}"
    echo "   Revisa los mensajes de error arriba"
    exit 1
fi
