window.blazorTabs = {

    dropdownComponents: [],
    dynamicTabSetComponents: [],

    ////////////
    // TabSet //
    ////////////

    registerDynamicTabSetComponent: (tabSet, div, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dynamicTabSetComponents.push({ tabSet: tabSet, div: div, component: component, componentGuid: componentGuid });
        }
    },

    setDynamicTabSetComponentHeight: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        dynamicTabSetComponent.component.invokeMethodAsync('SetContentHeight', height);
    },

    getDynamicTabSetComponentHeight: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        return height;
    },

    scrollLeftDynamicTabSet: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        dynamicTabSetComponent.tabSet.scrollLeft -= 50;
    },

    scrollRightDynamicTabSet: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        dynamicTabSetComponent.tabSet.scrollLeft += 50;
    },

    setActiveTab: (componentGuid, tabIndex) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];

        let tabSet = dynamicTabSetComponent.tabSet;
        let firstTabSetItem = tabSet.children[1];
        let activeTabSetItem = tabSet.children[tabIndex + 1];

        let firstTabSetItemRelativeOffsetLeft = firstTabSetItem.offsetLeft - tabSet.offsetLeft;
        let activeTabSetItemRelativeOffsetLeft = activeTabSetItem.offsetLeft - tabSet.offsetLeft;

        // scroll to left if needed
        let leftScrollOffset = firstTabSetItemRelativeOffsetLeft + tabSet.scrollLeft;
        if (activeTabSetItemRelativeOffsetLeft < leftScrollOffset) {
            let leftScrollDelta = leftScrollOffset - activeTabSetItemRelativeOffsetLeft;
            tabSet.scrollLeft -= leftScrollDelta;
        }

        // scroll to right if needed
        let rightScrollButton = tabSet.children[tabSet.children.length - 1];
        let rightScrollButtonRelativeOffsetLeft = rightScrollButton.offsetLeft - tabSet.offsetLeft;
        if (activeTabSetItemRelativeOffsetLeft + activeTabSetItem.offsetWidth > rightScrollButtonRelativeOffsetLeft) {
            let rightScrollDelta = activeTabSetItemRelativeOffsetLeft + activeTabSetItem.offsetWidth - rightScrollButtonRelativeOffsetLeft;
            tabSet.scrollLeft += rightScrollDelta;
        }
    },

    updateScrollButtons: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];

        // calculate the width of all tab set buttons
        let tabSet = dynamicTabSetComponent.tabSet;
        let tabButtonsWidth = 0;
        for (let i = 1; i < tabSet.children.length - 1; i++) {
            tabButtonsWidth += tabSet.children[i].scrollWidth;
        }

        // set style of scroll buttons
        let leftScrollButton = tabSet.children[0];
        let rightScrollButton = tabSet.children[tabSet.children.length - 1];
        if (tabButtonsWidth > tabSet.offsetWidth) {
            leftScrollButton.style.display = 'initial';
            rightScrollButton.style.display = 'initial';

            leftScrollButton.disabled = tabSet.scrollLeft === 0;
            rightScrollButton.disabled = tabSet.scrollLeft + tabSet.offsetWidth >= tabSet.scrollWidth;
        }
        else {
            leftScrollButton.style.display = 'none';
            rightScrollButton.style.display = 'none';
        }
    },

    scrollToLastTab: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];

        let tabSet = dynamicTabSetComponent.tabSet;
        tabSet.scrollLeft = tabSet.scrollWidth - tabSet.offsetWidth;
    },

    //////////////
    // Dropdown //
    //////////////

    registerDropdownComponent: (button, dropdownMenu, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dropdownComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dropdownComponents.push({ button: button, dropdownMenu: dropdownMenu, component: component, componentGuid: componentGuid });
        }
    },

    ///////////
    // Tools //
    ///////////

    unregisterDropdownComponent: (componentGuid) => {
        blazorTabs.unregisterComponent(blazorTabs.dropdownComponents, componentGuid);
    },

    unregisterComponent: (components, componentGuid) => {
        let index = blazorTabs.findComponentIndex(components, componentGuid);
        if (index !== -1) {
            components.splice(index, 1);
        }
    },

    findComponentIndex: (components, componentGuid) => {
        let index = -1;
        for (let i = 0; i < components.length; i++) {
            if (components[i].componentGuid === componentGuid) {
                index = i;
                break;
            }
        }

        return index;
    },
}

////////////
// Events //
////////////

window.addEventListener('mouseup', function (event) {
    for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
        let dropdownComponent = blazorTabs.dropdownComponents[i];
        if (event.target !== dropdownComponent.button && event.target.parentNode !== dropdownComponent.button) {
            if (dropdownComponent.dropdownMenu.classList.contains('show')) {
                dropdownComponent.component.invokeMethodAsync('CloseDropdown');
            }
        }
    }
});

window.addEventListener("resize", function () {
    for (let i = 0; i < blazorTabs.dynamicTabSetComponents.length; i++) {
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[i];
        blazorTabs.setDynamicTabSetComponentHeight(dynamicTabSetComponent.componentGuid);
        blazorTabs.updateScrollButtons(dynamicTabSetComponent.componentGuid);
    }
});