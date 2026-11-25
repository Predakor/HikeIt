#!/bin/sh
# One-time per clone: point git at the versioned hooks.
cd "$(git rev-parse --show-toplevel)"
git config core.hooksPath .githooks
chmod +x .githooks/pre-commit .githooks/pre-push
echo "hooks installed"
