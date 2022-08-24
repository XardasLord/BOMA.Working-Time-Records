import { Directive, ElementRef, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription, fromEvent, Subject, Observable } from 'rxjs';
import { map, debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Directive({
	selector: 'input[appDebounce]'
})
export class DebounceDirective implements OnInit, OnDestroy {
	@Output() public valueChanged: Observable<string>;

	private valueChangedSubject: Subject<string> = new Subject();
	private debounceTime = 500;
	private subscription: Subscription | undefined;

	constructor(private element: ElementRef) {
		this.valueChanged = this.valueChangedSubject.asObservable();
	}

	public ngOnInit(): void {
		this.subscription = fromEvent(this.element.nativeElement, 'keyup')
			.pipe(
				map(() => this.element.nativeElement.value),
				debounceTime(this.debounceTime),
				distinctUntilChanged()
			)
			.subscribe((value) => this.valueChangedSubject.next(value));
	}

	public ngOnDestroy(): void {
		if (this.subscription) {
			this.subscription.unsubscribe();
		}
	}
}
