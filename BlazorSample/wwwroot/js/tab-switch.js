// Function to switch tabs
window.switchTab = (tabId) => {
    // Get all tab buttons and contents
    const tabButtons = document.querySelectorAll('[role="tab"]');
    const tabContents = document.querySelectorAll('.tab-pane');

    // Remove active class from all tabs and contents
    tabButtons.forEach(btn => {
        btn.classList.remove('active');
        btn.setAttribute('aria-selected', 'false');
    });

    tabContents.forEach(content => {
        content.classList.remove('show', 'active');
    });

    // Add active class to clicked tab
    const activeTab = document.getElementById(tabId);
    if (activeTab) {
        activeTab.classList.add('active');
        activeTab.setAttribute('aria-selected', 'true');

        // Add active class to corresponding content
        const ariaControls = activeTab.getAttribute('aria-controls');
        if (ariaControls) {
            const contentDiv = document.getElementById(ariaControls);
            if (contentDiv) {
                contentDiv.classList.add('show', 'active');
            }
        }
    }
};
