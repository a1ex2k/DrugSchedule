function setThemeToggler(elementName) {
    document.getElementById(elementName).addEventListener('click', () => {
        if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
            document.documentElement.setAttribute('data-bs-theme', 'light')
        }
        else {
            document.documentElement.setAttribute('data-bs-theme', 'dark')
        }
    })
}

