import { Component, computed, input } from '@angular/core';
import { DashboardWidget } from '../../models/dashboard-widget';
import { RecentEmployeesWidget } from '../../models/widgets/recent-employees-widget';
import { RouterLink } from '@angular/router';
import { EmployeeListCard } from '../../../../shared/employee-list-card/employee-list-card';

@Component({
  selector: 'app-recent-employees',
  imports: [EmployeeListCard],
  templateUrl: './recent-employees.html',
  styleUrl: './recent-employees.css',
})
export class RecentEmployees {
    readonly widget = input.required<DashboardWidget>();

    readonly data = computed(() =>
        this.widget().data as RecentEmployeesWidget
    );
}
