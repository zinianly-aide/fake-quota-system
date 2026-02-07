#!/bin/bash

# Fake Quota Management System - Automated Testing Script
# Updated for Peekaboo 3.0.0-beta3

set -e

echo "=========================================="
echo "  Fake Quota Management System Test"
echo "=========================================="
echo ""

# Configuration
BACKEND_URL="http://localhost:8080"
FRONTEND_URL="http://localhost:8081"
SNAPSHOTS_DIR="./screenshots"

# Create screenshots directory
mkdir -p "$SNAPSHOTS_DIR"

# Function to launch Safari
launch_safari() {
    echo "🌐 Launching Safari..."
    peekaboo app --json << 'EOF'
{
  "action": "launch",
  "name": "Safari"
}
EOF'
    sleep 3
}

# Function to capture screenshot
capture_screenshot() {
    local filename=$1
    local description=$2
    
    echo "📸 Capturing: $description"
    peekaboo see --app Safari --annotate --path "$SNAPSHOTS_DIR/$filename"
    sleep 2
}

# Function to click element
click_element() {
    local query=$1
    local description=$2
    
    echo "🖱  Clicking: $description"
    peekaboo click --app Safari --query "$query"
    sleep 2
}

# Step 1: Launch Safari
echo "=========================================="
echo "Step 1: Launch Safari"
echo "=========================================="
launch_safari
capture_screenshot "01-homepage.png" "Homepage"
echo ""

# Step 2: Navigate to Dashboard
echo "=========================================="
echo "Step 2: Navigate to Dashboard"
echo "=========================================="
click_element "系统概览" "Navigate to Dashboard"
capture_screenshot "02-dashboard.png" "Dashboard"
echo ""

# Step 3: Navigate to Quota Management
echo "=========================================="
echo "Step 3: Navigate to Quota Management"
echo "=========================================="
click_element "额度管理" "Navigate to Quota Management"
capture_screenshot "03-quota-management.png" "Quota Management"
echo ""

# Step 4: Navigate to Employee Management
echo "=========================================="
echo "Step 4: Navigate to Employee Management"
echo "=========================================="
click_element "员工管理" "Navigate to Employee Management"
capture_screenshot "04-employee-management.png" "Employee Management"
echo ""

# Step 5: Test API Health Check
echo "=========================================="
echo "Step 5: Test API Health Check"
echo "=========================================="
# Open new tab for API
peekaboo app --json << 'EOF'
{
  "action": "launch",
  "name": "Safari"
}
EOF'
sleep 2

# Navigate to API health endpoint
curl -s "$BACKEND_URL/api/health" | python3 -m json.tool
capture_screenshot "05-api-health.png" "API Health Check"
echo ""

# Step 6: Summary
echo "=========================================="
echo "  Test Summary"
echo "=========================================="
echo "✅ Safari launched"
echo "✅ Dashboard tested"
echo "✅ Quota Management tested"
echo "✅ Employee Management tested"
echo "✅ API Health Check completed"
echo "✅ Screenshots saved to: $SNAPSHOTS_DIR"
echo ""
echo "Test completed successfully! 🎊"
echo "=========================================="
