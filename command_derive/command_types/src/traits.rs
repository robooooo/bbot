use framework::CommandContext;

/// Marker trait used to assert type equality in proc-macro.
pub trait IsResult {}
impl<T, U> IsResult for Result<T, U> {}

pub trait IsContext {}
impl IsContext for CommandContext {}