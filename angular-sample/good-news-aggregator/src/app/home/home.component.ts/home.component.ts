import {Component, computed, signal, untracked} from '@angular/core';

@Component({
  selector: 'app-home.component.ts',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent
{
  counter = signal(0);

  counter2 = computed(()=> {
    return untracked(this.counter) * 2;
  });


  constructor() {
    console.log(`counter value ${this.counter()}`);
  }

  incrementCounter():void {
    this.counter.set(this.counter() + 1); //set = set the value of the signal to the new value of the same type

    console.log(`Updating counter value to ${this.counter()}`);
  }

  incrementCounterUsingUpdate():void {
    this.counter.update(counter => counter + 1); //set = update
    console.log(`Updating counter value to ${this.counter()}`);
  }

  //why do we need to use signals

}
