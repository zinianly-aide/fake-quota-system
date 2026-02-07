# Fake Quota Management System - Automation Testing Guide

## 🚀 Overview

This guide explains how to use Peekaboo 3.0.0-beta3's new features for browser automation and testing.

## 📋 Table of Contents

- [Peekaboo 3.0.0-beta3 New Features](#peekaboo-300-beta3-new-features)
- [Quick Start](#quick-start)
- [Advanced Features](#advanced-features)
- [Script Updates](#script-updates)
- [Examples](#examples)
- [Troubleshooting](#troubleshooting)

---

## 🔍 Peekaboo 3.0.0-beta3 New Features

### 1. Application Targeting

**Old way:**
```bash
# Capture all screens
peekaboo see --path screenshot.png
```

**New way (Recommended):**
```bash
# Target specific application
peekaboo see --app Safari --path screenshot.png

# Use application alias
peekaboo see --app "Google Chrome" --path screenshot.png
```

### 2. JSON Output

**Why use JSON?**
- Better error handling
- Programmatic parsing
- Integration with automation tools

**Examples:**
```bash
# Capture with JSON output
peekaboo see --app Safari --json

# All tools support JSON
peekaboo click --app Safari --json
peekaboo type --app Safari --json
```

### 3. Snapshot Tracking

**Why use snapshots?**
- Track UI state changes
- Revert to specific states
- Debug complex flows

**Examples:**
```bash
# Capture with snapshot
peekaboo see --app Safari --snapshot "state_1"

# Use snapshot in later actions
peekaboo click --app Safari --snapshot "state_1" --text "Submit"
```

### 4. Pointer Tools

**New commands:**
```bash
# Move cursor
peekaboo move --app Safari --x 100 --y 200

# Drag and drop
peekaboo drag --app Safari --from_x 100 --from_y 200 --to_x 300 --to_y 400

# Scroll
peekaboo scroll --app Safari --direction down --amount 100

# Swipe (mobile testing)
peekaboo swipe --app Safari --direction right --distance 100
```

### 5. Application Control

**JSON format (Recommended):**
```bash
# Launch application
peekaboo app --json '{"action":"launch","name":"Safari"}'

# Switch to Chrome
peekaboo app --json '{"action":"switch","to":"Google Chrome"}'

# Focus window
peekaboo app --json '{"action":"focus","app":"Safari"}'

# Quit application
peekaboo app --json '{"action":"quit","name":"Safari"}'
```

### 6. Window Management

**Window operations:**
```bash
# Set window bounds
peekaboo window --json '{"action":"set-bounds","app":"Safari","x":0,"y":0,"width":1280,"height":720}'

# Resize window
peekaboo window --json '{"action":"resize","app":"Safari","width":1920,"height":1080}'

# Move window
peekaboo window --json '{"action":"move","app":"Safari","x":100,"y":100}'

# Minimize/Maximize
peekaboo window --json '{"action":"minimize","app":"Safari"}'
peekaboo window --json '{"action":"maximize","app":"Safari"}'
```

---

## 🚀 Quick Start

### Step 1: Install Peekaboo 3.0.0-beta3

```bash
# Check version
peekaboo --version

# Update to latest version
brew upgrade peekaboo
```

### Step 2: Basic Browser Automation

```bash
# 1. Launch Safari
peekaboo app --json '{"action":"launch","name":"Safari"}'

# 2. Navigate to URL
peekaboo open "http://localhost:8081"

# 3. Capture screenshot
peekaboo see --app Safari --path /tmp/homepage.png

# 4. Click element
peekaboo click --app Safari --query "Submit"
```

---

## 🔧 Advanced Features

### Multi-Window Management

```bash
# 1. Launch two browsers
peekaboo app --json '{"action":"launch","name":"Safari"}'
peekaboo app --json '{"action":"launch","name":"Google Chrome"}'

# 2. Switch between them
peekaboo app --json '{"action":"switch","to":"Google Chrome"}'
peekaboo app --json '{"action":"switch","to":"Safari"}'

# 3. Focus specific windows
peekaboo window --action focus --app "Google Chrome"
```

### Snapshot-Based Workflows

```bash
# 1. Capture initial state
peekaboo see --app Safari --snapshot "initial_state" --path /tmp/01-initial.png

# 2. Perform actions
peekaboo type --app Safari --text "Hello"

# 3. Capture modified state
peekaboo see --app Safari --snapshot "modified_state" --path /tmp/02-modified.png

# 4. Revert to initial state (if needed)
peekaboo click --app Safari --snapshot "initial_state" --text "Reset"
```

### Pointer-Based Interactions

```bash
# 1. Move cursor to element
peekaboo move --app Safari --x 500 --y 300

# 2. Click element (using pointer)
peekaboo click --app Safari --x 500 --y 300

# 3. Scroll using mouse wheel
peekaboo scroll --app Safari --direction down --amount 100

# 4. Drag element (if needed)
peekaboo drag --app Safari --from_x 100 --from_y 100 --to_x 300 --to_y 200
```

---

## 📋 Script Updates

### Old Script vs New Script

**Old script (basic automation):**
```bash
#!/bin/bash
# Old: Fuzzy commands
peekaboo see Safari
peekaboo click "Submit"
```

**New script (advanced automation):**
```bash
#!/bin/bash
# New: Application targeting and JSON output
peekaboo see --app Safari --json
peekaboo click --app Safari --query "Submit" --snapshot "submit_click"
```

### Best Practices

1. **Always use `--app` parameter**: Targets specific application, avoids ambiguity
2. **Use `--json` output**: Better error handling and parsing
3. **Use snapshots**: Track UI state for debugging
4. **Combine commands**: Chain multiple actions for efficiency
5. **Handle errors**: Parse JSON output to detect failures

---

## 🎯 Examples

### Example 1: API Testing Workflow

```bash
#!/bin/bash
# 1. Launch Safari
peekaboo app --json '{"action":"launch","name":"Safari"}'

# 2. Navigate to API
peekaboo open "http://localhost:8080/api/health"

# 3. Capture response
peekaboo see --app Safari --path /tmp/health-check.png --json

# 4. Test endpoint
curl -s http://localhost:8080/api/health

# 5. Capture result
peekaboo see --app Safari --path /tmp/test-result.png
```

### Example 2: Form Testing

```bash
#!/bin/bash
# 1. Launch application
peekaboo app --json '{"action":"launch","name":"Safari"}'
peekaboo open "http://localhost:8081"

# 2. Fill form
peekaboo type --app Safari --text "test@example.com" --field "email"
peekaboo type --app Safari --text "password123" --field "password"

# 3. Submit form
peekaboo click --app Safari --query "Submit" --snapshot "before_submit"

# 4. Capture result
peekaboo see --app Safari --path /tmp/after-submit.png --json
```

### Example 3: Multi-Page Testing

```bash
#!/bin/bash
# 1. Navigate to first page
peekaboo open "http://localhost:8081/quota"
peekaboo see --app Safari --snapshot "page_1"

# 2. Navigate to second page
peekaboo open "http://localhost:8081/employee"
peekaboo see --app Safari --snapshot "page_2"

# 3. Navigate to third page
peekaboo open "http://localhost:8081/application"
peekaboo see --app Safari --snapshot "page_3"

# 4. Navigate to fourth page
peekaboo open "http://localhost:8081/usage"
peekaboo see --app Safari --snapshot "page_4"
```

---

## 🐛 Troubleshooting

### Issue 1: Application not found

**Problem:**
```bash
peekaboo see --app Safari
# Error: Application not found
```

**Solution:**
```bash
# 1. List applications
peekaboo app --json '{"action":"list"}'

# 2. Check if app is running
peekaboo app --json '{"action":"list","running":true}'

# 3. Launch app if not running
peekaboo app --json '{"action":"launch","name":"Safari"}'
```

### Issue 2: Element click failed

**Problem:**
```bash
peekaboo click --app Safari --query "Submit"
# Element not found
```

**Solution:**
```bash
# 1. Capture screenshot to see current state
peekaboo see --app Safari --path /debug/click-failed.png

# 2. Try fuzzy matching
peekaboo click --app Safari --text "Submit"

# 3. Try snapshot-based click
peekaboo click --app Safari --snapshot "previous_state" --text "Submit"

# 4. Try coordinate-based click
peekaboo click --app Safari --x 500 --y 300
```

### Issue 3: Window management failed

**Problem:**
```bash
peekaboo window --json '{"action":"focus","app":"Safari"}'
# Error: Cannot find window
```

**Solution:**
```bash
# 1. List windows
peekaboo window --json '{"action":"list"}'

# 2. Use window_id if available
peekaboo window --json '{"action":"focus","window_id":"WINDOW_ID"}'

# 3. Use title if available
peekaboo window --json '{"action":"focus","title":"Safari - Fake Quota System"}'
```

---

## 📚 Resources

- [Peekaboo Documentation](https://github.com/peekaboo/peekaboo)
- [Peekaboo GitHub](https://github.com/peekaboo/peekaboo/releases)
- [Peekaboo Changelog](https://github.com/peekaboo/peekaboo/blob/main/CHANGELOG.md)

---

## 🎯 Next Steps

1. Update existing scripts to use new Peekaboo features
2. Implement snapshot tracking for state management
3. Use JSON output for better error handling
4. Combine multiple commands for efficiency
5. Test with both Safari and Google Chrome

---

**Created by OpenClaw Agent**
**Last Updated**: 2026-02-07
**Peekaboo Version**: 3.0.0-beta3
