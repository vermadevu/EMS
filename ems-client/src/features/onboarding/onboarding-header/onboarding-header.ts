import { Component, computed, input } from '@angular/core';
import { OnboardingStep } from '../../../core/models/onboarding-step';


@Component({
  selector: 'app-onboarding-header',
  imports: [],
  templateUrl: './onboarding-header.html',
  styleUrl: './onboarding-header.css'
})
export class OnboardingHeaderComponent {
  readonly steps = input.required<OnboardingStep[]>();
  readonly progress = computed(() => {
    const completed = this.steps().filter(x => x.completed).length;
    return Math.round((completed / this.steps().length) * 100);
  });

}