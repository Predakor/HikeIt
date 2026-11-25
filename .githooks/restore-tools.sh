#!/bin/sh
# CSharpier is the csharpier-expanded fork: fetch its package from the fork's GitHub Release into a local NuGet feed, then restore.
cd "$(git rev-parse --show-toplevel)"
version=$(grep -A2 '"csharpier"' .config/dotnet-tools.json | grep '"version"' | sed -E 's/.*"version": "([^"]+)".*/\1/')
feed="${CSHARPIER_FEED:-$HOME/.csharpier-expanded}"
package="$feed/CSharpier.$version.nupkg"

if [ "$(dotnet csharpier --version 2>/dev/null)" = "$version" ]; then
  exit 0
fi

if [ ! -f "$package" ]; then
  mkdir -p "$feed"
  curl -fsSL -o "$package" "https://github.com/Predakor/csharpier-expanded/releases/download/v$version/CSharpier.$version.nupkg" || rm -f "$package"
fi

dotnet tool restore --add-source "$(cygpath -w "$feed" 2>/dev/null || echo "$feed")" -v q
