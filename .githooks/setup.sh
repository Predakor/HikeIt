#!/bin/sh
# One-time per clone: point git at the versioned hooks and restore the dotnet tools.
cd "$(git rev-parse --show-toplevel)"
git config core.hooksPath .githooks
chmod +x .githooks/pre-commit .githooks/pre-push .githooks/restore-tools.sh
sh .githooks/restore-tools.sh

# The Visual Studio extension installs the pinned version from NuGet sources, so the fork's feed must be a machine-level source.
feed="${CSHARPIER_FEED:-$HOME/.csharpier-expanded}"
if ! dotnet nuget list source | grep -q "csharpier-expanded"; then
  dotnet nuget add source "$(cygpath -w "$feed" 2>/dev/null || echo "$feed")" -n csharpier-expanded
fi
echo "hooks installed"
