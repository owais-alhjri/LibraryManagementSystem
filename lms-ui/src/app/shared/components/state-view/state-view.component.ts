import { Component, Input } from '@angular/core';
import { LoadingSpinnerComponent } from '../confirm-dialog/loading-spinner/loading-spinner.component';
import { EmptyStateComponent } from '../empty-state/empty-state-component';

export type ViewState = 'loading' | 'success' | 'empty' | 'error';

@Component({
  selector: 'app-state-view',
  standalone: true,
  imports: [LoadingSpinnerComponent, EmptyStateComponent],
  template: `
    @switch (state) {
      @case ('loading') {
        <app-loading-spinner [message]="loadingMessage" />
      }
      @case ('error') {
        <app-empty-state icon="error_outline" title="Something went wrong" [message]="errorMessage" />
      }
      @case ('empty') {
        <app-empty-state [icon]="emptyIcon" [title]="emptyTitle" [message]="emptyMessage" />
      }
      @case ('success') {
        <ng-content></ng-content>
      }
    }
  `
})
export class StateViewComponent {
  @Input({ required: true }) state!: ViewState;
  @Input() loadingMessage = 'Loading...';
  @Input() errorMessage = 'Please try again.';
  @Input() emptyIcon = 'inbox';
  @Input() emptyTitle = 'Nothing here yet';
  @Input() emptyMessage = '';
}