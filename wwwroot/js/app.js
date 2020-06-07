window.blazorTabs = {

    dropdownComponents: [],

    registerDropdownComponent: (button, dropdownMenu, component) => {
        let index = blazorTabs.findDropdownComponentIndex(component);
        if (index === -1) {
            blazorTabs.dropdownComponents.push({ button: button, dropdownMenu: dropdownMenu, component: component });
        }
    },

    unregisterDropdownComponent: (component) => {
        let index = blazorTabs.findDropdownComponentIndex(component);
        if (index !== -1) {
            blazorTabs.dropdownComponents.splice(index, 1);
        }
    },

    findDropdownComponentIndex: (component) => {
        let index = -1;
        for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
            if (blazorTabs.dropdownComponents[i].component._id === component._id) {
                index = i;
                break;
            }
        }

        return index;
    },
}

window.addEventListener('mouseup', function (event) {
    for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
        let dropdownComponent = blazorTabs.dropdownComponents[i];
        if (event.target != dropdownComponent.button && event.target.parentNode != dropdownComponent.button) {
            if (dropdownComponent.dropdownMenu.classList.contains('show')) {
                dropdownComponent.component.invokeMethodAsync('CloseDropdown');
            }
        }
    }
});
