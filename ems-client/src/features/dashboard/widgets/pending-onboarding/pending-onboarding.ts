import { Component, computed, input } from '@angular/core';
import { EmployeeListCard } from '../../../../shared/employee-list-card/employee-list-card';
import { DashboardWidget } from '../../models/dashboard-widget';
import { PendingOnboardingWidget } from '../../models/widgets/pending-onboarding-widget';

@Component({
  selector: 'app-pending-onboarding',
  imports: [EmployeeListCard],
  templateUrl: './pending-onboarding.html',
  styleUrl: './pending-onboarding.css',
})
export class PendingOnboarding {
      readonly widget = input.required<DashboardWidget>();

    readonly data = computed(() =>
        this.widget().data as PendingOnboardingWidget
    );
}
