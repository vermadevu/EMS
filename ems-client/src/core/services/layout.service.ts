import { Service, signal } from '@angular/core';

@Service()
export class LayoutService {
    readonly sidebarOpen = signal(true);
    readonly pageTitle = signal('Dashboard');

    toggleSidebar(): void {
    this.sidebarOpen.update(open => !open);
    }

    openSidebar(): void {
    this.sidebarOpen.set(true);
    }

    closeSidebar(): void {
    this.sidebarOpen.set(false);
    }

    setPageTitle(title: string): void {
    this.pageTitle.set(title);
    }
}
