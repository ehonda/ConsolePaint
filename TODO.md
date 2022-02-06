__NEXT__

* Pass time since beginning to renderable (See TODO in oscillator.Step)
* Extract main loop into class, refactor demos to use it

__TODO__

* __Tests for Periodic version of covering__
* Tests for all the screens / rendering related stuff
* Make screens composable
* Resolve `TODO` comments
* Tests for Blinking Cursor
* InputHandler using InputQueue *-> not clear how to best do this yet*
* Main loop *-> not clear how to best do this yet*
* Better organization, better names, especially for the rendering stuff
* Add Game Of Life demo
* Add Console Paint demo

__PLAN__

* `UIComponent`
  * Consists of `Render()`, `InputHandler`, `HasFocus` etc.
  * `UI -> [UIComponent]`, orders them, `Render -> CompositeScreen` (via `Spectre.Console.Grid`?), `CompositeInputHandler`, manages focus, etc.
  * `ComponentWithBlinkingCursor` -> has blinking cursor, manages movement, disables show on lost focus, etc.