using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static String;

    /// <inheritdoc />
    public class MessageStatementVisitorFieldNumberingStrategy
        : DescriptorVisitorFieldNumberingStrategyBase<MessageStatement>
    {
        internal MessageStatementVisitorFieldNumberingStrategy(MessageStatement statement)
            : base(statement)
        {
        }

        private void Visit(IRangeDescriptor descriptor)
        {
            string notation;
            if (!AllowedFieldNumberRanges.All(range => range.Contains(descriptor)))
            {
                // TODO: TBD: add more descriptive bits...
                notation = Join(", ", AllowedFieldNumberRanges.Select(y => y.ToRangeNotation()));
                throw new InvalidOperationException($"{descriptor.ToRangeNotation()} out of valid range(s) {notation}.");
            }

            if (!DefaultReservedRanges.Any(range => range.TryIntersect(descriptor, out _)))
            {
                return;
            }

            notation = Join(", ", DefaultReservedRanges.Where(range => range.TryIntersect(descriptor, out _))
                .Select(range => range.ToRangeNotation()));
            throw new InvalidOperationException(
                $"{descriptor.ToRangeNotation()} cannot fall within reserved range {notation}.");
        }

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Gets a new <see cref="RangeDescriptor.Minimum"/> based <see cref="IComparer{T}"/>
        /// instance.
        /// </summary>
        private static IComparer<RangeDescriptor> MinimumComparer => new RangeDescriptorMinimumComparer { };

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Gets a new <see cref="RangeDescriptor.Maximum"/> based <see cref="IComparer{T}"/>
        /// instance.
        /// </summary>
        private static IComparer<RangeDescriptor> MaximumComparer => new RangeDescriptorMaximumComparer { };

        private void Visit<TBody>(TBody body)
            where TBody : IHasBody<RangeDescriptor>
            => body.Items
                .OrderBy(x => x, MinimumComparer)
                .ThenBy(x => x, MaximumComparer).ToList<IRangeDescriptor>().ForEach(Visit);

        // TODO: TBD: this would work for "a" Statement...
        // TODO: TBD: consider what we do for multiple Message Statements...
        // TODO: TBD: also consider how about Nested Message Statements...
        /// <inheritdoc />
        public override void Visit(MessageStatement statement)
        {
            // TODO: TBD: and if we need more specific information, consider how we pass that in...
            void VisitNumbers(IEnumerable<IHasNumber> items)
                => items.Select(x => new RangeDescriptor {Minimum = x.Number})
                    .OrderBy(x => x, MinimumComparer)
                    .ThenBy(x => x, MaximumComparer).ToList<IRangeDescriptor>().ForEach(Visit);

            VisitNumbers(statement.Items.OfType<IHasNumber>());

            /* Both of which support a Body of Range Descriptors,
             * and which must Satisfy the Range Descriptor Visitation. */

            statement.Items.OfType<ExtensionsStatement>().ToList<IHasBody<RangeDescriptor>>().ForEach(Visit);
            //                     ^^^^^^^^^^^^^^^^^^^

            statement.Items.OfType<RangesReservedStatement>().ToList<IHasBody<RangeDescriptor>>().ForEach(Visit);
            //                     ^^^^^^^^^^^^^^^^^^^^^^^
        }
    }
}
