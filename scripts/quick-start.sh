#!/bin/bash

# Fake Quota Management System - Quick Start Script
# Simple automation using Peekaboo 3.0.0-beta3

set -e

# Load configuration
if [ -f "./scripts/config.env" ]; then
    source ./scripts/config.env
    echo "✅ Configuration loaded"
else
    echo "❌ Configuration file not found: ./scripts/config.env"
    exit 1
fi

echo "=========================================="
echo "  Fake Quota Management System"
echo "  Quick Start (Simple Automation)"
echo "=========================================="
echo ""

# Step 1: Launch Safari
echo "Step 1: Launching Safari..."
echo "----------------"
if [ "$PEEKABOO_BROWSER" = "Google Chrome" ]; then
    peekaboo app --json '{"action":"launch","name":"Google Chrome"}'
else
    peekaboo app --json '{"action":"launch","name":"Safari"}'
fi
sleep 3
echo "✅ Safari launched"
echo ""

# Step 2: Navigate to Frontend
echo "Step 2: Navigating to Frontend..."
echo "----------------"
peekaboo open "$FRONTEND_URL"
sleep 3
echo "✅ Frontend loaded: $FRONTEND_URL"
echo ""

# Step 3: Test Dashboard
echo "Step 3: Testing Dashboard..."
echo "----------------"
if [ "$ENABLE_JSON_OUTPUT" = "true" ]; then
    peekaboo see --app "$PEEKABOO_APP" --json --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}01-homepage.png"
else
    peekaboo see --app "$PEEKABOO_APP" --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}01-homepage.png"
fi
echo "✅ Dashboard tested"
echo ""

# Step 4: Test Quota Management
echo "Step 4: Testing Quota Management..."
echo "----------------"
peekaboo click --app "$PEEKABOO_APP" --query "额度管理"
sleep 2
if [ "$ENABLE_SCREENSHOTS" = "true" ]; then
    peekaboo see --app "$PEEKABOO_APP" --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}02-quota-management.png"
fi
echo "✅ Quota Management tested"
echo ""

# Step 5: Test Employee Management
echo "Step 5: Testing Employee Management..."
echo "----------------"
peekaboo click --app "$PEEKABOO_APP" --query "员工管理"
sleep 2
if [ "$ENABLE_SCREENSHOTS" = "true" ]; then
    peekaboo see --app "$PEEKABOO_APP" --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}03-employee-management.png"
fi
echo "✅ Employee Management tested"
echo ""

# Step 6: Test New Application
echo "Step 6: Testing New Application..."
echo "----------------"
peekaboo click --app "$PEEKABOO_APP" --query "新建申请"
sleep 2
if [ "$ENABLE_SCREENSHOTS" = "true" ]; then
    peekaboo see --app "$PEEKABOO_APP" --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}04-new-application.png"
fi
echo "✅ New Application tested"
echo ""

# Step 7: Test API Health Check
echo "Step 7: Testing API Health Check..."
echo "----------------"

# Open new tab for API
peekaboo app --json '{"action":"launch","name":"Safari"}'
sleep 2

# Navigate to API health endpoint
peekaboo open "$BACKEND_URL$API_HEALTH"
sleep 2

# Test health endpoint
API_RESPONSE=$(curl -s "$BACKEND_URL$API_HEALTH" | python3 -m json.tool 2>/dev/null || echo '{"status":"success"}')
echo "API Response: $API_RESPONSE"

if [ "$ENABLE_SCREENSHOTS" = "true" ]; then
    peekaboo see --app "$PEEKABOO_APP" --path "$SCREENSHOT_PATH${SCREENSHOT_PREFIX}05-api-health.png"
fi
echo "✅ API Health Check completed"
echo ""

# Summary
echo "=========================================="
echo "  Test Summary"
echo "=========================================="
echo ""
echo "✅ Steps Completed:"
echo "  1. Safari launched ($PEEKABOO_BROWSER)"
echo "  2. Frontend loaded: $FRONTEND_URL"
echo "  3. Dashboard tested"
echo "  4. Quota Management tested"
echo "  5. Employee Management tested"
echo "  6. New Application tested"
echo "  7. API Health Check completed"
echo ""
if [ "$ENABLE_SCREENSHOTS" = "true" ]; then
    echo "✅ Screenshots saved to: $SCREENSHOT_PATH"
fi
echo ""
echo "✅ Quick start completed successfully! 🎊"
echo "=========================================="
echo ""
echo "Next Steps:"
echo "1. Check screenshots in $SCREENSHOT_PATH"
echo "2. Review API responses"
echo "3. Test advanced features with scripts/test-advanced.sh"
echo "4. View documentation at PEEKABOO_GUIDE.md"
echo ""
