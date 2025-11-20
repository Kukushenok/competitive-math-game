#!/usr/bin/env bash
set -e

THRESHOLD=1500   # допустимая Halstead Total (можешь настроить)
SRC_DIR="./src"
DB_FILE="metrixpp.db"

echo "🧮 Running Halstead complexity analysis with Metrix++..."

# Удаляем старую БД
rm -f $DB_FILE

# Собираем метрики
metrix++ collect \
  --std.code.complexity.halstead \
  --std.code.complexity.cyclomatic \
  --std.code.lines \
  --std.code.comments \
  -- $SRC_DIR > /dev/null

# Просматриваем результаты, ищем нарушения
violations=$(metrix++ view --db-file=$DB_FILE --filter complexity.halstead.total\>$THRESHOLD | grep -E "\.cs" || true)

if [ -n "$violations" ]; then
    echo "❌ Found files exceeding Halstead complexity threshold ($THRESHOLD):"
    echo "$violations"
    exit 1
else
    echo "✅ Halstead complexity OK (<= $THRESHOLD)"
fi
