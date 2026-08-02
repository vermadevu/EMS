import { Component, computed, inject, signal } from '@angular/core';
import { NotificationService } from '../../../core/services/notification-service';
import { OnboardingService } from '../../../core/services/onboarding-service';
import { Router } from '@angular/router';
import { PolicyItem } from '../../../core/models/policy-item';

@Component({
  selector: 'app-ploicies-step',
  imports: [],
  templateUrl: './ploicies-step.html',
  styleUrl: './ploicies-step.css',
})
export class PloiciesStepComponent {
  readonly accepted = signal(false);
  private readonly notificationService = inject(NotificationService);
  private readonly onboardingService = inject(OnboardingService);
  private readonly router = inject(Router);

  readonly policies = signal<PolicyItem[]>([
    {
      id: 1,
      title: 'Code of Conduct',
      description: 'Maintain professional behaviour at work.',
      accepted: false
    },
    {
      id: 2,
      title: 'Information Security',
      description: 'Protect company information and credentials.',
      accepted: false
    },
    {
      id: 3,
      title: 'IT Asset Usage',
      description: 'Use company devices responsibly.',
      accepted: false
    },
    {
      id: 4,
      title: 'Confidentiality Agreement',
      description: 'Do not disclose confidential information.',
      accepted: false
    },
    {
      id: 5,
      title: 'Workplace Ethics',
      description: 'Respect diversity and workplace standards.',
      accepted: false
    }
  ]);

  readonly confirmationAccepted = signal(false);

  readonly canContinue = computed(() => {
    return this.policies().every(x => x.accepted)
      && this.confirmationAccepted();

  });


  continue(): void {
    if (!this.canContinue()) {
      this.notificationService.error(
        'Please accept all policies before continuing.'
      );
      return;
    }
    this.onboardingService.complete(
      '/onboarding/policies'
    );
    this.router.navigate([
      '/onboarding/review'
    ]);
  }

  togglePolicy(id: number, checked: boolean): void {
    this.policies.update(policies =>
      policies.map(policy =>
        policy.id === id
          ? {
            ...policy,
            accepted: checked
          }
          : policy
      )
    );
  }
}
