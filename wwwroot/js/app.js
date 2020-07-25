window.blazorTabs = {

    dropdownComponents: [],
    dynamicTabSetComponents: [],

    ////////////
    // TabSet //
    ////////////

    registerDynamicTabSetComponent: (tabSet, div, btnLeftScroll, btnRightScroll, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dynamicTabSetComponents.push(
            {
                tabSet: tabSet,
                div: div,
                btnLeftScroll: btnLeftScroll,
                btnRightScroll: btnRightScroll,
                component: component,
                componentGuid: componentGuid
            });
        }
    },

    unregisterDynamicTabSetComponent: (componentGuid) => {
        blazorTabs.unregisterComponent(blazorTabs.dynamicTabSetComponents, componentGuid);
    },

    getDynamicTabSetComponent (componentGuid) {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        return dynamicTabSetComponent;
    },

    setDynamicTabSetComponentHeight: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        dynamicTabSetComponent.component.invokeMethodAsync('SetContentHeight', height);
    },

    getDynamicTabSetComponentHeight: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        return height;
    },

    scrollLeftDynamicTabSet: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        return setInterval(blazorTabs.dynamicTabSetScrollLoop, 10, dynamicTabSetComponent.tabSet, componentGuid, -5);
    },

    scrollRightDynamicTabSet: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        return setInterval(blazorTabs.dynamicTabSetScrollLoop, 10, dynamicTabSetComponent.tabSet, componentGuid, 5);
    },

    stopDynamicTabSetScrolling: (dynamicTabSetScrollLoopId) => {
        clearInterval(dynamicTabSetScrollLoopId);
    },

    dynamicTabSetScrollLoop: (tabSet, componenetGuid, delta) => {
        tabSet.scrollLeft += delta;

        const [isLeftScrollButtonDisabled, isRightScrollButtonDisabled] = blazorTabs.updateScrollButtons(componenetGuid);
        if ((delta < 0 && isLeftScrollButtonDisabled) || (delta > 0 && isRightScrollButtonDisabled)) {
            let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componenetGuid);
            dynamicTabSetComponent.component.invokeMethodAsync('GetScrollLoopId').then(dynamicTabSetScrollLoopId => {
                clearInterval(dynamicTabSetScrollLoopId);
            });
        }
    },

    setActiveTab: (componentGuid, tabIndex) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        let tabSet = dynamicTabSetComponent.tabSet;
        let firstTabSetItem = tabSet.children[0];
        let activeTabSetItem = tabSet.children[tabIndex];

        let firstTabSetItemRelativeOffsetLeft = firstTabSetItem.offsetLeft - tabSet.offsetLeft;
        let activeTabSetItemRelativeOffsetLeft = activeTabSetItem.offsetLeft - tabSet.offsetLeft;

        // scroll to left if needed
        let leftScrollOffset = firstTabSetItemRelativeOffsetLeft + tabSet.scrollLeft;
        if (activeTabSetItemRelativeOffsetLeft < leftScrollOffset) {
            let leftScrollDelta = leftScrollOffset - activeTabSetItemRelativeOffsetLeft;
            tabSet.scrollLeft -= leftScrollDelta;
        }

        // scroll to right if needed
        let btnRightScroll = dynamicTabSetComponent.btnRightScroll;
        let rightScrollButtonRelativeOffsetLeft = btnRightScroll .offsetLeft - tabSet.offsetLeft;
        if (activeTabSetItemRelativeOffsetLeft + activeTabSetItem.offsetWidth > rightScrollButtonRelativeOffsetLeft) {
            let rightScrollDelta = activeTabSetItemRelativeOffsetLeft + activeTabSetItem.offsetWidth - rightScrollButtonRelativeOffsetLeft;
            tabSet.scrollLeft += rightScrollDelta;
        }
    },

    updateScrollButtons: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);

        // calculate the width of all tab set buttons
        let tabSet = dynamicTabSetComponent.tabSet;
        let tabButtonsWidth = 0;
        for (let i = 0; i < tabSet.children.length; i++) {
            tabButtonsWidth += tabSet.children[i].scrollWidth;
        }

        // set style of scroll buttons
        let btnLeftScroll = dynamicTabSetComponent.btnLeftScroll;
        let btnRightScroll = dynamicTabSetComponent.btnRightScroll;
        if (tabButtonsWidth > tabSet.offsetWidth) {
            btnLeftScroll.style.display = 'initial';
            btnRightScroll.style.display = 'initial';

            btnLeftScroll.disabled = tabSet.scrollLeft === 0;
            btnRightScroll.disabled = tabSet.scrollLeft + tabSet.offsetWidth >= tabSet.scrollWidth;
            btnRightScroll.style.borderLeft = btnRightScroll.disabled ? "none" : null;
        }
        else {
            btnLeftScroll.style.display = 'none';
            btnRightScroll.style.display = 'none';
        }

        return [btnLeftScroll.disabled, btnRightScroll.disabled];
    },

    scrollToLastTab: (componentGuid) => {
        let dynamicTabSetComponent = blazorTabs.getDynamicTabSetComponent(componentGuid);
        let tabSet = dynamicTabSetComponent.tabSet;
        tabSet.scrollLeft = tabSet.scrollWidth;
        blazorTabs.updateScrollButtons(componentGuid);
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

    unregisterDropdownComponent: (componentGuid) => {
        blazorTabs.unregisterComponent(blazorTabs.dropdownComponents, componentGuid);
    },

    ///////////
    // Tools //
    ///////////

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