import { computed, Service, signal } from '@angular/core';
import { OnboardingStep } from '../models/onboarding-step';

@Service()
export class OnboardingService {
    readonly steps = signal<OnboardingStep[]>([
        {
            id: 1,
            title: 'Profile',
            icon: 'person',
            route: '/onboarding/profile',
            completed: false
        },
        {
            id: 2,
            title: 'Documents',
            icon: 'description',
            route: '/onboarding/documents',
            completed: false
        },
        {
            id: 3,
            title: 'Assets',
            icon: 'inventory_2',
            route: '/onboarding/assets',
            completed: false
        },
        {
            id: 4,
            title: 'Policies',
            icon: 'gavel',
            route: '/onboarding/policies',
            completed: false
        },
        {
            id: 5,
            title: 'Review',
            icon: 'check_circle',
            route: '/onboarding/review',
            completed: false
        }
    ]);

    readonly progress = computed(() => {
        const completed = this.steps()
            .filter(x => x.completed).length;
        return Math.round(
            (completed / this.steps().length) * 100
        );
    });

    complete(route: string): void {
        this.steps.update(steps =>
            steps.map(step =>
                step.route === route
                    ? { ...step, completed: true }
                    : step
            )
        );
    }
}
