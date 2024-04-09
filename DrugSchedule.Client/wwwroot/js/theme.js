let darkMode = localStorage.getItem("dark-mode");

const enableDarkMode = () => {
    document.documentElement.setAttribute('data-bs-theme', 'dark')
    localStorage.setItem("dark-mode", "enabled");
};

const disableDarkMode = () => {
    document.documentElement.setAttribute('data-bs-theme', 'light')
    localStorage.setItem("dark-mode", "disabled");
};

if (darkMode === "enabled") {
    enableDarkMode(); 
}

function addThemeToggleClick() {
    const toggleBtn = document.getElementById("theme-toggle-btn");
    toggleBtn.addEventListener("click", (e) => {
        darkMode = localStorage.getItem("dark-mode");
        if (darkMode === "disabled") {
            enableDarkMode();
        } else {
            disableDarkMode();
        }
    });
} 