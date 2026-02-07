#!/bin/bash

# Fake Quota Management System - Advanced Automation Script
# Uses Peekaboo 3.0.0-beta3 new features
# Supports: snapshot tracking, pointer tools, JSON output, app targeting

set -e

echo "=========================================="
echo "  Fake Quota Management System"
echo "  Advanced Automation Script"
echo "=========================================="
echo ""

# Configuration
BACKEND_URL="http://localhost:8080"
FRONTEND_URL="http://localhost:8081"
SCREENSHOTS_DIR="./screenshots/advanced"
APP_NAME="Safari"

# Create screenshots directory
mkdir -p "$SCREENSHOTS_DIR"

# Initialize snapshot tracking
declare -a SNAPSHOTS=()

# Function to take and track screenshot
track_screenshot() {
    local filename=$1
    local description=$2
    local snapshot_id
    
    echo "📸 Capturing: $description"
    
    # Use Peekaboo 3.0.0-beta3 with snapshot tracking
    peekaboo see --app "$APP_NAME" --annotate --path "$SCREENSHOTS_DIR/$filename" --json-output
    
    # Store snapshot info
    snapshot_id=$(date +%s)
    SNAPSHOTS+=("$snapshot_id:$filename:$description")
    
    sleep 2
    echo "✅ Snapshot saved: $filename (ID: $snapshot_id)"
    echo ""
}

# Function to launch app with JSON
launch_app() {
    local app_name=$1
    
    echo "🚀 Launching: $app_name"
    
    # Use JSON output for better error handling
    peekaboo app --json << EOF
{
  "action": "launch",
  "name": "$app_name"
}
EOF
    
    sleep 3
    echo "✅ App launched: $app_name"
    echo ""
}

# Function to switch app
switch_app() {
    local target_app=$1
    
    echo "🔄 Switching to: $target_app"
    
    peekaboo app --json << EOF
{
  "action": "switch",
  "to": "$target_app"
}
EOF
    
    sleep 3
    echo "✅ Switched to: $target_app"
    echo ""
}

# Function to click with query
click_element() {
    local query=$1
    local description=$2
    
    echo "🖱 Clicking: $description (Query: $query)"
    
    peekaboo click --app "$APP_NAME" --query "$query"
    
    sleep 2
    echo "✅ Clicked: $description"
    echo ""
}

# Function to click with snapshot
click_with_snapshot() {
    local query=$1
    local description=$2
    local filename=$3
    
    echo "🖱 Clicking: $description"
    
    # Click and use snapshot tracking
    peekaboo click --app "$APP_NAME" --query "$query" --snapshot "snap_$(date +%s)" --json-output
    
    sleep 2
    
    # Capture screenshot with new snapshot ID
    track_screenshot "$filename" "$description"
}

# Function to type text
type_text() {
    local text=$1
    local description=$2
    
    echo "⌨️ Typing: $description"
    
    peekaboo type --app "$APP_NAME" --text "$text"
    
    sleep 2
    echo "✅ Typed: $text"
    echo ""
}

# Function to scroll
scroll_page() {
    local direction=$1
    local amount=$2
    
    echo "📜 Scrolling: $direction ($amount pixels)"
    
    peekaboo scroll --direction "$direction" --amount "$amount" --app "$APP_NAME"
    
    sleep 1
    echo "✅ Scrolled"
    echo ""
}

# Function to move cursor
move_cursor() {
    local x=$1
    local y=$2
    
    echo "🖱 Moving cursor to: ($x, $y)"
    
    peekaboo move --app "$APP_NAME" --x "$x" --y "$y" --profile "human"
    
    sleep 1
    echo "✅ Moved cursor"
    echo ""
}

# Function to focus window
focus_window() {
    local window_name=$1
    
    echo "🔍 Focusing window: $window_name"
    
    peekaboo window --action focus --app "$APP_NAME" --window-name "$window_name"
    
    sleep 2
    echo "✅ Window focused: $window_name"
    echo ""
}

# Function to resize window
resize_window() {
    local app_name=$1
    local width=$2
    local height=$3
    
    echo "📐 Resizing window: $app_name to ${width}x${height}"
    
    peekaboo window --action set-bounds --app "$app_name" --width "$width" --height "$height"
    
    sleep 2
    echo "✅ Window resized: ${width}x${height}"
    echo ""
}

# ========================================
# Test Scenario: Full User Flow
# ========================================

echo "=========================================="
echo "  Scenario 1: Full User Flow"
echo "=========================================="
echo ""

# Step 1: Launch Safari
echo "Step 1: Launch Safari"
echo "----------------"
launch_app "Safari"
track_screenshot "01-launch.png" "Launch Safari"

# Step 2: Navigate to Frontend
echo "Step 2: Navigate to Frontend"
echo "----------------"
peekaboo open "$FRONTEND_URL"
sleep 5

# Wait for page load and capture
track_screenshot "02-frontend-loaded.png" "Frontend Loaded"

# Step 3: Navigate to Dashboard
echo "Step 3: Navigate to Dashboard"
echo "----------------"
click_element "系统概览" "Click Dashboard"
track_screenshot "03-dashboard.png" "Dashboard"

# Step 4: Navigate to Quota Management
echo "Step 4: Navigate to Quota Management"
echo "----------------"
click_element "额度管理" "Click Quota Management"
track_screenshot "04-quota-management.png" "Quota Management"

# Step 5: Navigate to Employee Management
echo "Step 5: Navigate to Employee Management"
echo "----------------"
click_element "员工管理" "Click Employee Management"
track_screenshot "05-employee-management.png" "Employee Management"

# Step 6: Navigate to New Application
echo "Step 6: Navigate to New Application"
echo "----------------"
click_element "新建申请" "Click New Application"
track_screenshot "06-new-application.png" "New Application"

# Step 7: Test API
echo "Step 7: Test API Health Check"
echo "----------------"

# Open new tab for API
peekaboo app --json << EOF
{
  "action": "launch",
  "name": "Safari"
}
EOF

sleep 3

# Navigate to API
peekaboo open "$BACKEND_URL/api/health"
sleep 5

# Capture API response
track_screenshot "07-api-health.png" "API Health Check"

# ========================================
# Test Scenario: Pointer Tools
# ========================================

echo ""
echo "=========================================="
echo "  Scenario 2: Pointer Tools Test"
echo "=========================================="
echo ""

# Navigate to home
peekaboo open "$FRONTEND_URL"
sleep 5

# Test cursor movement
echo "Test 1: Cursor Movement"
echo "----------------"
move_cursor 500 300
track_screenshot "08-cursor-moved.png" "Cursor Moved"

# Test scroll
echo "Test 2: Page Scroll"
echo "----------------"
scroll_page "down" 500
track_screenshot "09-page-scrolled.png" "Page Scrolled"

# Test click
echo "Test 3: Element Click"
echo "----------------"
click_element "系统概览" "Click Dashboard"
track_screenshot "10-dashboard-clicked.png" "Dashboard Clicked"

# ========================================
# Test Scenario: Snapshot Tracking
# ========================================

echo ""
echo "=========================================="
echo "  Scenario 3: Snapshot Tracking"
echo "=========================================="
echo ""

echo "Using snapshot tracking for automation state management"
echo "Available snapshots:"
for snapshot in "${SNAPSHOTS[@]}"; do
    IFS=':' read -r id filename desc <<< "$snapshot"
    echo "  - ID: $id, File: $filename, Desc: $desc"
done
echo ""

# ========================================
# Test Scenario: Window Management
# ========================================

echo "=========================================="
echo "  Scenario 4: Window Management"
echo "=========================================="
echo ""

# Focus browser window
focus_window "Safari"
track_screenshot "11-window-focused.png" "Window Focused"

# Resize window (for testing)
resize_window "Safari" 1200 800
track_screenshot "12-window-resized.png" "Window Resized"

# ========================================
# Test Scenario: JSON Output
# ========================================

echo ""
echo "=========================================="
echo "  Scenario 5: JSON Output"
echo "=========================================="
echo ""

echo "Capturing UI state with JSON output"
peekaboo see --app "$APP_NAME" --json-output --path "$SCREENSHOTS_DIR/ui-state.json"

echo ""
echo "=========================================="
echo "  Test Summary"
echo "=========================================="
echo ""
echo "✅ Scenarios Completed:"
echo "  1. Full User Flow"
echo "  2. Pointer Tools Test"
echo "  3. Snapshot Tracking"
echo "  4. Window Management"
echo "  5. JSON Output"
echo ""
echo "✅ Screenshots saved to: $SCREENSHOTS_DIR"
echo "✅ Snapshots tracked: ${#SNAPSHOTS[@]} snapshots"
echo ""
echo "Test completed successfully! 🎊"
echo "=========================================="
