import { Component, inject } from '@angular/core';
import { DashboardService } from '../../core/services/dashboard.service';
import { AsyncPipe } from '@angular/common';
import { DashboardWidgetComponent } from './dashboard-widget/dashboard-widget';

@Component({
  selector: 'app-dashboard',
  imports: [
    AsyncPipe,
    DashboardWidgetComponent
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardComponent {
  private readonly dashboardService = inject(DashboardService);

  readonly dashboard$ = this.dashboardService.getDashboard();
}
