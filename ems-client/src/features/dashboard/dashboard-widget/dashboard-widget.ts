import { Component, input } from '@angular/core';
import { StatisticCard } from '../widgets/statistic-card/statistic-card';
import { RecentEmployees } from '../widgets/recent-employees/recent-employees';
import { DashboardWidget } from '../models/dashboard-widget';
import { DashboardWidgetType } from '../models/dashboard-widget-type';
import { PendingOnboarding } from '../widgets/pending-onboarding/pending-onboarding';

@Component({
  selector: 'app-dashboard-widget',
  imports: [
    StatisticCard,
    RecentEmployees,
    PendingOnboarding
],
  templateUrl: './dashboard-widget.html',
  styleUrl: './dashboard-widget.css',
})
export class DashboardWidgetComponent {
    readonly widget = input.required<DashboardWidget>();
    protected readonly DashboardWidgetType = DashboardWidgetType;
}
