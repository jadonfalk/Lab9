**How the assignment patterns are implemented**

**Pattern:** Object Pool
**Implementation:** BulletPool creates bullets once, lends them out, and accepts returned bullets.

**Pattern:** Builder
**Implementation:** TargetDirector uses ITargetBuilder steps to configure different targets through TargetBuilder.

**Pattern:** Observer
**Implementation:** Target publishes a hit event. ScoreManager subscribes and updates the score.
