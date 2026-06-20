import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [MatIconModule],
  template: `
    <div class="empty-state">
      <mat-icon class="empty-icon">{{ icon }}</mat-icon>
      <h3>{{ title }}</h3>
      @if (message) {
        <p>{{ message }}</p>
      }
      <ng-content></ng-content> <!-- optional action button slot, e.g. "Add Book" -->
    </div>
  `,
  styles: [`
    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 48px 16px;
      text-align: center;
      gap: 8px;
    }
    .empty-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      color: rgba(0,0,0,.38);
    }
  `]
})
export class EmptyStateComponent {
  @Input() icon = 'inbox';
  @Input() title = '';
  @Input() message = '';
}