import { Component, computed, input, OnInit } from '@angular/core';
import { DashboardWidget } from '../../models/dashboard-widget';
import { RecentEmployeesWidget } from '../../models/widgets/recent-employees-widget';
import { EmployeeListCard } from '../../../../shared/components/employee-list-card/employee-list-card';
import { DashboardWidgetType } from '../../models/dashboard-widget-type';
import { PendingApproval } from "../../pending-approval/pending-approval";

@Component({
  selector: 'app-recent-employees',
  imports: [EmployeeListCard, PendingApproval],
  templateUrl: './recent-employees.html',
  styleUrl: './recent-employees.css',
})
export class RecentEmployees {
    readonly widget = input.required<DashboardWidget>();
    readonly  DashboardWidgetType = DashboardWidgetType;

    readonly data = computed(() =>
        this.widget().data as RecentEmployeesWidget
    );
}
